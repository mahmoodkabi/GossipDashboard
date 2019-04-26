using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GossipDashboard.Models;

namespace GossipDashboard.ViewModel
{
    public class VM_PostManage
    {
        //[ReadOnly(true)]
        public int PostID { get; set; }


        [DisplayName("عنوان")]
        [Required]
        public string Subject1 { get; set; }
        public string SubSubject1_1 { get; set; }
        public string SubSubject1_2 { get; set; }
        public string ContentPost1_1 { get; set; }
        public string ContentPost1_2 { get; set; }
        public string ContentPost1_3 { get; set; }
        public string ContentPost1_4 { get; set; }
        public string ContentPost1_5 { get; set; }


        [DisplayName("عکس")]
        [Required]
        public string Image1_1 { get; set; }
        public string Image1_2 { get; set; }
        public string Image1_3 { get; set; }
        public string Subject2 { get; set; }
        public string SubSubject2_1 { get; set; }
        public string SubSubject2_2 { get; set; }
        public string ContentPost2_1 { get; set; }
        public string ContentPost2_2 { get; set; }
        public string ContentPost2_3 { get; set; }
        public string ContentPost2_4 { get; set; }
        public string ContentPost2_5 { get; set; }
        public string Image2_1 { get; set; }
        public string Image2_2 { get; set; }
        public string Image2_3 { get; set; }
        public string Subject3 { get; set; }
        public string SubSubject3_1 { get; set; }
        public string SubSubject3_2 { get; set; }
        public string ContentPost3_1 { get; set; }
        public string ContentPost3_2 { get; set; }
        public string ContentPost3_3 { get; set; }
        public string ContentPost3_4 { get; set; }
        public string ContentPost3_5 { get; set; }
        public string Image3_1 { get; set; }
        public string Image3_2 { get; set; }
        public string Image3_3 { get; set; }
        public string Subject4 { get; set; }
        public string SubSubject4_1 { get; set; }
        public string SubSubject4_2 { get; set; }
        public string ContentPost4_1 { get; set; }
        public string ContentPost4_2 { get; set; }
        public string ContentPost4_3 { get; set; }
        public string ContentPost4_4 { get; set; }
        public string ContentPost4_5 { get; set; }
        public string Image4_1 { get; set; }
        public string Image4_2 { get; set; }
        public string Image4_3 { get; set; }
        public string Subject5 { get; set; }
        public string SubSubject5_1 { get; set; }
        public string SubSubject5_2 { get; set; }
        public string ContentPost5_1 { get; set; }
        public string ContentPost5_2 { get; set; }
        public string ContentPost5_3 { get; set; }
        public string ContentPost5_4 { get; set; }
        public string ContentPost5_5 { get; set; }
        public string Image5_1 { get; set; }
        public string Image5_2 { get; set; }
        public string Image5_3 { get; set; }
        public string Subject6 { get; set; }
        public string SubSubject6_1 { get; set; }
        public string SubSubject6_2 { get; set; }
        public string ContentPost6_1 { get; set; }
        public string ContentPost6_2 { get; set; }
        public string ContentPost6_3 { get; set; }
        public string ContentPost6_4 { get; set; }
        public string ContentPost6_5 { get; set; }
        public string Image6_1 { get; set; }
        public string Image6_2 { get; set; }
        public string Image6_3 { get; set; }
        public string Subject7 { get; set; }
        public string SubSubject7_1 { get; set; }
        public string SubSubject7_2 { get; set; }
        public string ContentPost7_1 { get; set; }
        public string ContentPost7_2 { get; set; }
        public string ContentPost7_3 { get; set; }
        public string ContentPost7_4 { get; set; }
        public string ContentPost7_5 { get; set; }
        public string Image7_1 { get; set; }
        public string Image7_2 { get; set; }
        public string Image7_3 { get; set; }
        public string Subject8 { get; set; }
        public string SubSubject8_1 { get; set; }
        public string SubSubject8_2 { get; set; }
        public string ContentPost8_1 { get; set; }
        public string ContentPost8_2 { get; set; }
        public string ContentPost8_3 { get; set; }
        public string ContentPost8_4 { get; set; }
        public string ContentPost8_5 { get; set; }
        public string Image8_1 { get; set; }
        public string Image8_2 { get; set; }
        public string Image8_3 { get; set; }
        public string Subject9 { get; set; }
        public string SubSubject9_1 { get; set; }
        public string SubSubject9_2 { get; set; }
        public string ContentPost9_1 { get; set; }
        public string ContentPost9_2 { get; set; }
        public string ContentPost9_3 { get; set; }
        public string ContentPost9_4 { get; set; }
        public string ContentPost9_5 { get; set; }
        public string Image9_1 { get; set; }
        public string Image9_2 { get; set; }
        public string Image9_3 { get; set; }
        public string Subject10 { get; set; }
        public string SubSubject10_1 { get; set; }
        public string SubSubject10_2 { get; set; }
        public string ContentPost10_1 { get; set; }
        public string ContentPost10_2 { get; set; }
        public string ContentPost10_3 { get; set; }
        public string ContentPost10_4 { get; set; }
        public string ContentPost10_5 { get; set; }
        public string Image10_1 { get; set; }
        public string Image10_2 { get; set; }
        public string Image10_3 { get; set; }
        public string Subject11 { get; set; }
        public string SubSubject11_1 { get; set; }
        public string SubSubject11_2 { get; set; }
        public string ContentPost11_1 { get; set; }
        public string ContentPost11_2 { get; set; }
        public string ContentPost11_3 { get; set; }
        public string ContentPost11_4 { get; set; }
        public string ContentPost11_5 { get; set; }
        public string Image11_1 { get; set; }
        public string Image11_2 { get; set; }
        public string Image11_3 { get; set; }
        public string Subject12 { get; set; }
        public string SubSubject12_1 { get; set; }
        public string SubSubject12_2 { get; set; }
        public string ContentPost12_1 { get; set; }
        public string ContentPost12_2 { get; set; }
        public string ContentPost12_3 { get; set; }
        public string ContentPost12_4 { get; set; }
        public string ContentPost12_5 { get; set; }
        public string Image12_1 { get; set; }
        public string Image12_2 { get; set; }
        public string Image12_3 { get; set; }
        public string Subject13 { get; set; }
        public string SubSubject13_1 { get; set; }
        public string SubSubject13_2 { get; set; }
        public string ContentPost13_1 { get; set; }
        public string ContentPost13_2 { get; set; }
        public string ContentPost13_3 { get; set; }
        public string ContentPost13_4 { get; set; }
        public string ContentPost13_5 { get; set; }
        public string Image13_1 { get; set; }
        public string Image13_2 { get; set; }
        public string Image13_3 { get; set; }
        public string Subject14 { get; set; }
        public string SubSubject14_1 { get; set; }
        public string SubSubject14_2 { get; set; }
        public string ContentPost14_1 { get; set; }
        public string ContentPost14_2 { get; set; }
        public string ContentPost14_3 { get; set; }
        public string ContentPost14_4 { get; set; }
        public string ContentPost14_5 { get; set; }
        public string Image14_1 { get; set; }
        public string Image14_2 { get; set; }
        public string Image14_3 { get; set; }
        public string Subject15 { get; set; }
        public string SubSubject15_1 { get; set; }
        public string SubSubject15_2 { get; set; }
        public string ContentPost15_1 { get; set; }
        public string ContentPost15_2 { get; set; }
        public string ContentPost15_3 { get; set; }
        public string ContentPost15_4 { get; set; }
        public string ContentPost15_5 { get; set; }
        public string Image15_1 { get; set; }
        public string Image15_2 { get; set; }
        public string Image15_3 { get; set; }
        public string Subject16 { get; set; }
        public string SubSubject16_1 { get; set; }
        public string SubSubject16_2 { get; set; }
        public string ContentPost16_1 { get; set; }
        public string ContentPost16_2 { get; set; }
        public string ContentPost16_3 { get; set; }
        public string ContentPost16_4 { get; set; }
        public string ContentPost16_5 { get; set; }
        public string Image16_1 { get; set; }
        public string Image16_2 { get; set; }
        public string Image16_3 { get; set; }
        public string Subject17 { get; set; }
        public string SubSubject17_1 { get; set; }
        public string SubSubject17_2 { get; set; }
        public string ContentPost17_1 { get; set; }
        public string ContentPost17_2 { get; set; }
        public string ContentPost17_3 { get; set; }
        public string ContentPost17_4 { get; set; }
        public string ContentPost17_5 { get; set; }
        public string Image17_1 { get; set; }
        public string Image17_2 { get; set; }
        public string Image17_3 { get; set; }
        public string Subject18 { get; set; }
        public string SubSubject18_1 { get; set; }
        public string SubSubject18_2 { get; set; }
        public string ContentPost18_1 { get; set; }
        public string ContentPost18_2 { get; set; }
        public string ContentPost18_3 { get; set; }
        public string ContentPost18_4 { get; set; }
        public string ContentPost18_5 { get; set; }
        public string Image18_1 { get; set; }
        public string Image18_2 { get; set; }
        public string Image18_3 { get; set; }
        public string Subject19 { get; set; }
        public string SubSubject19_1 { get; set; }
        public string SubSubject19_2 { get; set; }
        public string ContentPost19_1 { get; set; }
        public string ContentPost19_2 { get; set; }
        public string ContentPost19_3 { get; set; }
        public string ContentPost19_4 { get; set; }
        public string ContentPost19_5 { get; set; }
        public string Image19_1 { get; set; }
        public string Image19_2 { get; set; }
        public string Image19_3 { get; set; }
        public string Subject20 { get; set; }
        public string SubSubject20_1 { get; set; }
        public string SubSubject20_2 { get; set; }
        public string ContentPost20_1 { get; set; }
        public string ContentPost20_2 { get; set; }
        public string ContentPost20_3 { get; set; }
        public string ContentPost20_4 { get; set; }
        public string ContentPost20_5 { get; set; }
        public string Image20_1 { get; set; }
        public string Image20_2 { get; set; }
        public string Image20_3 { get; set; }
        public string QuotedFrom { get; set; }
        public string Url { get; set; }
        public string UrlMP3 { get; set; }
        public string UrlVideo { get; set; }

        [DisplayName("اسکريپت آپارات")]
        [AllowHtml]
        public string ScriptAparat { get; set; }
        public Nullable<int> Views { get; set; }
        public Nullable<int> LikePost { get; set; }
        public Nullable<int> DislikePost { get; set; }
        public Nullable<int> PublishCount { get; set; }

        [DisplayName("بک گراند")]
        public string BackgroundColor { get; set; }
        public Nullable<int> ModifyUserID { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<bool> IsCreatedPost { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
        public string Cat3 { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string Tag6 { get; set; }
        public string Tag7 { get; set; }
        public string Tag8 { get; set; }
        public string Tag9 { get; set; }
        public string Tag10 { get; set; }
        public string FootCategory { get; set; }
        public string DateTimePost { get; set; }
        public string ContentPost1_6 { get; set; }
        public string ContentPost1_7 { get; set; }
        public string AboutUser { get; set; }
        public byte[] ImageUser { get; set; }
        public string JalaliModifyDate { get; set; }
        public string Fullname { get; set; }
        public int CommentCount { get; set; }
        public int UserID_fk { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DisplayName("آدرس سايت مرجع")]
        [Required]
        public string SourceSiteUrl { get; set; }


        public string SourceFootCategory { get; set; }
        public string SourceDateTimePost { get; set; }

        [DisplayName("نام سايت مرجع")]
        [Required]
        public string SourceSiteName { get; set; }
        public string SourceSiteNameFa { get; set; }
        [DisplayName("طبقه بندی")]
        public string PostCategoryName { get; set; }
        [DisplayName("فرمت")]
        public string PostFormatName { get; set; }
        [DisplayName("تعداد ستون")]
        public string PostColName { get; set; }
        [DisplayName("طبقه بندی")]
        public int PostCategoryID { get; set; }
        [DisplayName("فرمت")]
        public int PostFormatID { get; set; }
        [DisplayName("تعداد ستون")]
        public int PostColID { get; set; }

        [AllowHtml]
        [DisplayName("HTML محتوا")]
        [Required]
        public string ContentHTML { get; set; }

        [DisplayName("وضعيت")]
        public string Status { get; set; }
        [DisplayName("نويسنده وضعيت")]
        public string StatusAuthor { get; set; }

        public Nullable<System.DateTime> FisrtDatePostCreated { get; set; }
    }
}