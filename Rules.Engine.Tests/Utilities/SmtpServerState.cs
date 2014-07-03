using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rules.Engine.Tests.Utilities
{
    internal sealed class SmtpServerState
    {
        private const int _maxDataSize = 1024 * 1024;
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        private bool _terminate;
        private bool _receivedHELO;
        private bool _receivedMAIL;
        private bool _receivedRCPT;
        private SmtpRequest GetNextRequest()
        {
            Thread.Sleep(200);
            if (_terminate) return new SmtpRequest("QUIT");

            try
            {
                return new SmtpRequest(_reader.ReadLine());
            }
            catch (Exception ex)
            {
                return new SmtpRequest("QUIT " + ex.Message);
            }
        }
        private void RecordHELO()
        {
            _receivedHELO = true;
        }
        private void RecordMAIL(string message)
        {
            if (!message.Contains("<") || !message.Contains(">")) return;
            int startIndex = message.IndexOf('<') + 1;
            FromAddress = message.Substring(startIndex, message.LastIndexOf('>') - startIndex);
            _receivedMAIL = true;
        }
        private void RecordRCPT(string message)
        {
            if (!message.Contains("<") || !message.Contains(">")) return;
            int startIndex = message.IndexOf('<') + 1;
            ToAddresses = ToAddresses == null
                ? new[] { message.Substring(startIndex, message.LastIndexOf('>') - startIndex) }
                : ToAddresses.Union(new[] { message.Substring(startIndex, message.LastIndexOf('>') - startIndex) }).ToArray();
            _receivedRCPT = true;
        }
        private void RecordDATA()
        {
            StringBuilder sb = new StringBuilder();
            bool bodyStarted = false;
            string line;
            while (sb.Length < _maxDataSize)
            {
                line = _reader.ReadLine();
                if (line == null || line == ".")
                {
                    break;
                }
                if (bodyStarted)
                {
                    sb.Append(line);
                }
                else if (line.StartsWith("From:") && line.Contains("\""))
                {
                    int startIndex = line.IndexOf('"') + 1;
                    FromFriendlyName = line.Substring(startIndex, line.LastIndexOf('"') - startIndex);
                }
                else if (line.StartsWith("Subject:"))
                {
                    Subject = line.Substring(line.IndexOf(':') + 2);
                }
                else if (line.Length == 0)
                {
                    bodyStarted = true;
                }
            }

            Body = sb.ToString();
        }
        private void RecordRSET()
        {
            _receivedMAIL = false;
            _receivedRCPT = false;
            FromAddress = null;
            FromFriendlyName = null;
            ToAddresses = null;
            Subject = null;
            Body = null;
        }

        private void Send_220_Greeting()
        {
            _writer.WriteLine("220 localhost ESMTP SmtpServerSession 1.0");
            _writer.Flush();
        }
        private void Send_250_OK(params string[] messages)
        {
            if (messages == null || messages.Length == 0)
            {
                _writer.WriteLine("250 OK");
            }
            else
            {
                for (int i = 0; i < messages.Length - 1; i++)
                {
                    _writer.WriteLine("250-" + messages[i]);
                }
                _writer.WriteLine("250 " + messages[messages.Length - 1]);
            }
            _writer.Flush();
        }
        private void Send_354_EnterMessage()
        {
            _writer.WriteLine("354 Enter message, ending with \".\" on a line by itself");
            _writer.Flush();
        }
        private void Send_221_Closing()
        {
            if (!_writer.BaseStream.CanWrite) return;

            _writer.WriteLine("221 SmtpServerSession closing connection");
            _writer.Flush();
        }
        private void Send_214_Help()
        {
            _writer.WriteLine("214-Commands supported:");
            _writer.WriteLine("214 " + String.Join(" ", Enum.GetNames(typeof(SmtpRequestType)).Where(name => name != Error)));
            _writer.Flush();
        }
        private void Send_503_Error(string message)
        {
            _writer.WriteLine("503 " + message);
            _writer.Flush();
        }

        public SmtpServerState(NetworkStream stream)
        {
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);
        }
        public void Accept()
        {
            try
            {
                Send_220_Greeting();
                SmtpRequest request;
                do
                {
                    request = GetNextRequest();
                    switch (request.Code)
                    {
                        case SmtpRequestType.EHLO:
                            RecordHELO();
                            Send_250_OK("SmtpServerSession Hello", "SIZE " + _maxDataSize, "HELP");
                            break;
                        case SmtpRequestType.HELO:
                            RecordHELO();
                            Send_250_OK("SmtpServerSession Hello");
                            break;
                        case SmtpRequestType.MAIL:
                            if (_receivedHELO)
                            {
                                RecordMAIL(request.Message);
                                Send_250_OK();
                            }
                            else
                            {
                                Send_503_Error("send HELO or EHLO command first");
                            }
                            break;
                        case SmtpRequestType.RCPT:
                            if (_receivedMAIL)
                            {
                                RecordRCPT(request.Message);
                                Send_250_OK("Accepted");
                            }
                            else
                            {
                                Send_503_Error("sender not yet given. valid MAIL command must precede RCPT");
                            }
                            break;
                        case SmtpRequestType.DATA:
                            if (_receivedRCPT)
                            {
                                Send_354_EnterMessage();
                                RecordDATA();
                                Send_250_OK();
                            }
                            else
                            {
                                Send_503_Error("valid RCPT command must precede DATA");
                            }
                            break;
                        case SmtpRequestType.RSET:
                            RecordRSET();
                            Send_250_OK("Reset OK");
                            break;
                        case SmtpRequestType.NOOP:
                            Send_250_OK();
                            break;
                        case SmtpRequestType.HELP:
                            Send_214_Help();
                            break;
                        case SmtpRequestType.QUIT:
                            break;
                    }
                } while (!_terminate && request.Code != SmtpRequestType.QUIT);

                Send_221_Closing();
            }
            catch (Exception ex)
            {
                Error = ex.ToString();
            }
            finally
            {
                _reader.Dispose();
                _writer.Dispose();
            }
        }
        public void Terminate()
        {
            _terminate = true;
        }

        public bool HasError
        {
            get { return Error != null; }
        }
        public string Error { get; private set; }
        public string FromAddress { get; private set; }
        public string FromFriendlyName { get; private set; }
        public string[] ToAddresses { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
    }
}
