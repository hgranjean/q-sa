using System;

namespace Atum.Utility.TypeMapping
{
	/// <summary>
	/// Summary description for ITypeMapper.
	/// </summary>
	public interface ITypeMapper
	{
		string TranslateTypeA(string TypeB);
		string TranslateTypeB(string TypeA);
	}
}
