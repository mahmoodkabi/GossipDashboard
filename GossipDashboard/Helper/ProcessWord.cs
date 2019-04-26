using GossipDashboard.Models;
using NRakeCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Helper
{
    public class ProcessWord
    {
        public string FindKeywordsFromPost(string ContentHTMLText, List<string> wordFrequency)
        {
            if (ContentHTMLText == "")
                return "";

            List<Words> listKeyWord = new List<Words>();
            KabiExtractKeyword kabi = new KabiExtractKeyword();
            KeywordExtractor Extractor = new KeywordExtractor();

            ////ایجاد تا حد ممکن کلمات فارسی از یک متن طولانی
            ////var str = File.ReadAllText("Sample1.txt");
            ////var res = kabi.FindNoneImportantWord(str);
            ////var resFrequency = res.Select(p => new Frequency() { FrequencyWord = p.Word, FrequencyCount = p.Count }).ToList();
            ////context.Frequencies.AddRange(resFrequency);
            ////context.SaveChanges();

            //جدا کردن تمام کلمات یک پست بر اساس اسپیس
            var postAllWords = kabi.FindNoneImportantWord(ContentHTMLText);

            // به دست آوردن لیست جملات کلیدی پست
            var postImportantStatment = Extractor.FindKeyPhrases(ContentHTMLText);

            // جدا کردن لیست جملات کلیدی بر اساس اسپیس
            List<string> postImportantStatmentWords = new List<string>();
            foreach (var item in postImportantStatment)
            {
                postImportantStatmentWords.AddRange(item.Split(' '));
            }

            //ایجاد حلقه برای تک تک کلمات پست
            foreach (var itemPostAllWord in postAllWords)
            {
                //این کلمات جز کلمات حاصل از جملات کلیدی باشند
                if (postImportantStatmentWords.Contains(itemPostAllWord.Word))
                    // این کلمات جز کلمات پرتکرار فارسی نباشند
                    if (!wordFrequency.Contains(itemPostAllWord.Word))
                    {
                        //در این صورت به لیست اضافه کن
                        listKeyWord.Add(new Words() { Word = itemPostAllWord.Word, Count = itemPostAllWord.Count });
                    }
            }

            //بر اساس تعداد تکرار وصعودی مرتب کن و اولی ها را بردار
            listKeyWord = listKeyWord.OrderByDescending(p => p.Count).ToList();
            var finalKeywords = listKeyWord.Take(3).Select(p => new FrequencyWord() { FrequencyWord1 = p.Word, FrequencyCount = p.Count }).ToList();
            var strKeywords = "";
            foreach (var item in finalKeywords)
            {
                strKeywords += item.FrequencyWord1 + ",";
            }
            if (strKeywords.Length != 0)
                strKeywords = strKeywords.Substring(0, strKeywords.Length - 1);
            return strKeywords;
        }
    }

    class Words
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }
}