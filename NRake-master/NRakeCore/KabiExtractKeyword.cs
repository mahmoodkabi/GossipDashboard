using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NRakeCore
{
    public class KabiExtractKeyword
    {
        protected string Sample1
        {
            get
            {
                return File.ReadAllText("Sample1.txt");
            }
        }

        public List<Result> FindNoneImportantWord(string str)
        {
            //List<string> finalArr = new List<string>();

            //جدا کردن بر اساس اسپیس
            //var arr = this.Sample1.Split(' ');
            var arr = str.Split(' ');

            ////جدا کردن بر اساس نیم اسپیس
            //foreach (var item in arr)
            //{
            //    var arrItem = item.Split('‌');

            //    foreach (var item1 in arrItem)
            //    {
            //        finalArr.Add(item1);
            //    }
                
            //}

            var arrGroup = (from p in arr
                            group p by p into g
                           orderby g.Count() descending
                          select new Result { Word = g.Key, Count = g.Count() }).ToList();

            return arrGroup;
        }
    }

    public class Result
    {
        public string Word { get; set; }
        public int Count { get; set; }
    }

}
