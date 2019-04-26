using GossipDashboard.Helper;
using GossipDashboard.Models;
using GossipDashboard.Repository;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;


public static class Utilty
{
    public static string ToPersianDateTime(this DateTime? dt)
    {
        if (dt == null)
            return "";

        DateTime dt1 = (DateTime)dt;
        PersianCalendar pc = new PersianCalendar();
        int year = pc.GetYear(dt1);
        int month = pc.GetMonth(dt1);
        int day = pc.GetDayOfMonth(dt1);
        int hour = pc.GetHour(dt1);
        int minute = pc.GetMinute(dt1);


        string strDay = day.ToString(), strMonth = month.ToString(), strYear = year.ToString();
        if (strDay.Length == 1)
            strDay = "0" + strDay;
        if (strMonth.Length == 1)
            strMonth = "0" + strMonth;

        return strYear + "/" + strMonth + "/" + strDay + "  " + hour + ":" + minute;
    }



    /// <summary>
    /// در هر گروه، بالاترین مقادیر را یکی یکی خارج می کند و لیست جدید می سازد
    /// </summary>
    /// <param name="posts"></param>
    /// <returns></returns>
    //
    // گروه  A    B    C
    //       31   52    48
    //       29   37    49
    //       22   30    39
    // بعد از مرتب کردن
    // گروه  ListAll
    //          52
    //          49
    //          39
    //          48
    //          37
    //          30
    //          31
    //          29
    //          22
    public static List<VM_Post> SortGroupsList(List<VM_Post> posts, PlaceInMainPage placePost)
    {
        PostRepository repo = new PostRepository();
        ManagementPostRepository repoManagementPost = new ManagementPostRepository();
        PubBaseRepository repoPubBase = new PubBaseRepository();


        //ایجاد تگ آرتیکل به ازای هر پست
        List<VM_Post> listAll = new List<VM_Post>();
        List<VM_Post> listPostMaxGroup = new List<VM_Post>();
        int previousGroupPostMaxID = 100000000;
        VM_Post previousGroupPost = null, maxGroup = null, postMaxGroup = null;
        int i = 0;
        var postGroup = posts.Select(p => p.SourceSiteName).Distinct().ToList();
        do
        {
            listPostMaxGroup = new List<VM_Post>();

            //پس از گروه بندی بر اساس نام سایت
            foreach (var item in postGroup)
            {
                //کمترین مقداری را که برای این گروه سایت به لیست اد کرده پیدا می کند
                previousGroupPost = listAll.OrderBy(p => p.PostID).FirstOrDefault(p => p.SourceSiteName == item);
                previousGroupPostMaxID = previousGroupPost == null ? 100000000 : previousGroupPost.PostID;

                // اکنون بالاترین مقدار را برای گروه سایت به دست می آورد 
                //با شرط اینکه از آخرین مقدار وارد شده کوچکتر باشد
                maxGroup = posts.OrderByDescending(p => p.PostID).FirstOrDefault(p => p.SourceSiteName == item && p.PostID < previousGroupPostMaxID);
                if (maxGroup != null)
                {
                    postMaxGroup = posts.First(p => p.PostID == maxGroup.PostID);
                    if (postMaxGroup != null)
                        listPostMaxGroup.Add(postMaxGroup);
                }

                i++;
            }
            var listPostMaxGroupOrder = listPostMaxGroup.OrderByDescending(p => p.PostID);

            listAll.AddRange(listPostMaxGroupOrder);
        } while (i < posts.Count);


        //به قسمت صفحه اصلی  پست صوتی ، تصویری، استاتوس و لینک نیز اضافه می کنیم
        if(placePost == PlaceInMainPage.MiddleIndex)
        {
            //يک پست لينک اضافه مي کنيم
            if (listAll.Count >= 50)
            {
                //هر پستی را به عنوان پیوند می توان انتخاب کرد
                var link = listAll[50];

                if ((link.BackgroundColor == null ? "" : link.BackgroundColor.Trim()) == "")
                    link.BackgroundColor = "#cf3d2e";
                link.PostFormat.RemoveAt(0);
                link.PostFormat.Add(new VM_PubBase()
                {
                    PubBaseID = 26,
                    Active = true,
                    NameEn = "link",
                    ClassName = "format-link",
                    NameFa = "پیوند"
                });

                listAll.Insert(10, link);
            }


            //اضافه کردن پست استاتوس
            //با هر 4 پابلیش استاتوس قبلی حذف می شود و استاتوس جدید اضافه می گردد
            var status = repo.SelectPostUser().Where(p => p.Status != null && (p.PublishCount == null ? 0 : p.PublishCount) <= 4).OrderBy(p => p.PostID).FirstOrDefault();
            if (status != null)
            {
                listAll.Insert(15, status);
            }


            //اضافه کردن پست تصویری
            //با هر 4 پابلیش تصویری قبلی حذف می شود و تصویری جدید اضافه می گردد
            var video = repo.SelectPostUser().Where(p => p.UrlVideo != null && (p.PublishCount == null ? 0 : p.PublishCount) <= 4).OrderByDescending(p => p.PostID).FirstOrDefault();
            if (video != null)
            {
                listAll.Insert(20, video);
            }


            //اضافه کردن پست صوتی
            //با هر 4 پابلیش صوتی قبلی حذف می شود و صوتی جدید اضافه می گردد
            var mp3 = repo.SelectPostUser().Where(p => p.UrlMP3 != null && (p.PublishCount == null ? 0 : p.PublishCount) <= 4).OrderByDescending(p => p.PostID).FirstOrDefault();
            if (mp3 != null)
            {
                listAll.Insert(7, mp3);
            }
        }
       

        return listAll;
    }

    public static List<PropertiesImage> GetPropertiesImage(string path)
    {
        List<PropertiesImage> propImages = new List<PropertiesImage>();

        // Create an Image object. 
        Image theImage = new Bitmap(path);

        // Get the PropertyItems property from image.
        PropertyItem[] propItems = theImage.PropertyItems;

        foreach (PropertyItem propItem in propItems)
        {
            propImages.Add(new PropertiesImage()
            { 
                Id = propItem.Id,
                Value = propItem.Value,
                Type = propItem.Type,
                Len = propItem.Len,
            });
        }

        return propImages;
    }

    public static string GenerateSlug(string slug)
    {
        // invalid chars           
        slug = Regex.Replace(slug, @"[^\p{L}a-z0-9\s-]", "");
        // convert multiple spaces into one space   
        slug = Regex.Replace(slug, @"\s+", " ").Trim();
        // cut and trim 
        slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
        slug = Regex.Replace(slug, @"\s", "-"); // hyphens   
        return slug;
    }



    //public static  string GenerateSlug(int Id, string Name)
    //{
    //    string phrase = string.Format("{0}-{1}", Id, Name);

    //    string str = phrase;// RemoveAccent(phrase).ToLower();
    //    // invalid chars           
    //    str = Regex.Replace(str, @"[^\p{L}a-z0-9\s-]", "");
    //    // convert multiple spaces into one space   
    //    str = Regex.Replace(str, @"\s+", " ").Trim();
    //    // cut and trim 
    //    str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
    //    str = Regex.Replace(str, @"\s", "-"); // hyphens   
    //    return str;
    //}

    //private static string RemoveAccent(string text)
    //{
    //    byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
    //    return System.Text.Encoding.ASCII.GetString(bytes);
    //}
}

public class PropertiesImage
{
    public int Id { get; set; }
    public byte[] Value { get; set; }
    public short Type { get; set; }
    public int Len { get; set; }
}
