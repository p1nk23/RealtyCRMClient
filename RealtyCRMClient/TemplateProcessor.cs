using RealtyCRMClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealtyCRMClient
{
    public static class TemplateProcessor
    {
        public static string ReplaceFields(string template, DocumentFields fields)
        {
            var result = template;

            // Основные поля
            result = result.Replace("{SellerName},", $"гр. РФ {fields.SellerName},")
                           //.Replace("(фамилия, имя отчество)", fields.SellerName)
                           //.Replace("(адрес регистрации)", fields.SellerAddress)
                           .Replace("{BuyerName},", $"гр. РФ {fields.BuyerName},")
                           //.Replace("(адрес регистрации) ", fields.BuyerAddress)
                           .Replace("{цена цифрами}", fields.PriceNumeric)
                           .Replace("{цена словами}", fields.PriceInWords)
                           .Replace("{адрес квартиры}", fields.Address)
                           .Replace("{общая площадь}", fields.TotalArea);
                           //.Replace("{местонахождение банковской ячейки}", fields.BankCellLocation);

            // Условные блоки — выбор варианта
            if (fields.IsMarriedSeller && !fields.NotarizedConsentProvided)
            {
                result = result.Replace("4.1.9. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.", "");
                //result = RemoveSection(result,
                //    "4.1.9. Квартира не находится в общей совместной собственности супругов, в связи с чем получение какого-либо согласия на заключение настоящего Договора не требуется.",
                //    "4.1.9. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.");
            }
            else if (!fields.IsMarriedSeller)
            {
                result = result.Replace("4.1.9. Квартира не находится в общей совместной собственности супругов, в связи с чем получение какого-либо согласия на заключение настоящего Договора не требуется.", "");
                //result = RemoveSection(result,
                //    "4.1.9. Квартира не находится в общей совместной собственности супругов, в связи с чем получение какого-либо согласия на заключение настоящего Договора не требуется.",
                //    "4.1.9. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.");
            }

            if (fields.IsMarriedBuyer)
            {
                result = result.Replace("4.2.2. На момент заключения настоящего Договора Покупатель в браке не состоит.", "");
                //result = ReplaceSection(result,
                //    "4.2.2. На момент заключения настоящего Договора Покупатель в браке не состоит.",
                //    "4.2.2. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.");
            }
            else
            {
                result = result.Replace("4.2.2. Покупатель получил и представил нотариально удостоверенное согласие второго супруга на заключение настоящего Договора на установленных в нем условиях.", "");
            }

            return result;
        }

        private static string RemoveSection(string text, string startMarker, string endMarker)
        {
            int startIndex = text.IndexOf(startMarker);
            int endIndex = text.IndexOf(endMarker, startIndex) + endMarker.Length;

            if (startIndex >= 0 && endIndex > startIndex)
            {
                text = text.Remove(startIndex, endIndex - startIndex);
            }

            return text;
        }

        private static string ReplaceSection(string text, string oldText, string newText)
        {
            return text.Replace(oldText, newText);
        }
    }
}
