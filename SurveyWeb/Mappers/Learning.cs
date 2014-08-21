using Atum.Domain.NLP.NaiveBayes;
using SurveyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SurveyWeb.Mappers
{
    public class Learning
    {
        private void Mapping()
        {
            TrainingDocument tngDoc = new TrainingDocument();
            TrainingDocumentViewModel tngDocMV = new TrainingDocumentViewModel();
            tngDoc.DocumentId = tngDocMV.DocumentId;

            tngDoc.Words = tngDocMV.WordList;
            tngDoc.Class = tngDocMV.Class;
            
        }

        public static TrainingDocumentViewModel MapToViewModel(TrainingDocument trainingDocument)
        {
            TrainingDocumentViewModel retVal = new TrainingDocumentViewModel();
            retVal.DocumentId = string.IsNullOrEmpty(trainingDocument.DocumentId) ? trainingDocument.Class : trainingDocument.DocumentId;
            retVal.WordList = trainingDocument.Words;
            retVal.Text = trainingDocument.Text;
            retVal.Class = trainingDocument.Class;

            return retVal;
        }

        internal static TrainingDocument MapToDomainModel(TrainingDocumentViewModel model, Atum.Domain.NLP.Tokenizer Tokenizer)
        {
            TrainingDocument tngDoc = new TrainingDocument(model.Class,model.Text,Tokenizer);
            TrainingDocumentViewModel tngDocMV = model;
            tngDoc.DocumentId = tngDocMV.DocumentId;

            //tngDoc.Words = tngDocMV.WordList;
            //tngDoc.Class = tngDocMV.Class;
            return tngDoc;
        }

    }
}