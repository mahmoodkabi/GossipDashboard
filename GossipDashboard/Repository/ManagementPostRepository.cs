using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class ManagementPostRepository : IRepository<VM_PostManage>
    {
        private GossipSiteEntities context = new GossipSiteEntities();

        public IQueryable<VM_PostManage> ReadPost()
        {
            var postCategoryID = context.PubBases.First(p => p.NameEn == "PostCategory").PubBaseID;
            var postFormatID = context.PubBases.First(p => p.NameEn == "PostFormat").PubBaseID;
            var PostColID = context.PubBases.First(p => p.NameEn == "PostCol").PubBaseID;

            var res = from P in context.Posts
                      join UP in context.UserPosts on P.PostID equals UP.PostID_fk
                      join U in context.Users on UP.UserID_fk equals U.UserID
                      select new VM_PostManage
                      {
                          PostID = P.PostID,
                          Subject1 = P.Subject1,
                          SubSubject1_1 = P.SubSubject1_1,
                          SubSubject1_2 = P.SubSubject1_2,
                          ContentPost1_1 = P.ContentPost1_1,
                          ContentPost1_2 = P.ContentPost1_2,
                          ContentPost1_3 = P.ContentPost1_3,
                          ContentPost1_4 = P.ContentPost1_4,
                          ContentPost1_5 = P.ContentPost1_5,
                          Image1_1 = P.Image1_1,
                          Image1_2 = P.Image1_2,
                          Image1_3 = P.Image1_3,
                          Url = P.Url,
                          UrlMP3 = P.UrlMP3,
                          UrlVideo = P.UrlVideo,
                          Views = P.Views,
                          LikePost = P.LikePost,
                          DislikePost = P.DislikePost,
                          PublishCount = P.PublishCount,
                          BackgroundColor = P.BackgroundColor,
                          ModifyUserID = P.ModifyUserID,
                          ModifyDate = P.ModifyDate,
                          IsCreatedPost = P.IsCreatedPost,
                          Cat1 = P.Cat1,
                          Cat2 = P.Cat2,
                          Cat3 = P.Cat3,
                          Tag1 = P.Tag1,
                          Tag2 = P.Tag2,
                          Tag3 = P.Tag3,
                          Tag4 = P.Tag4,
                          Tag5 = P.Tag5,
                          FootCategory = P.SourceFootCategory,
                          DateTimePost = P.SourceDateTimePost,
                          SourceDateTimePost = P.SourceDateTimePost,
                          SourceFootCategory = P.SourceFootCategory,
                          SourceSiteName = P.SourceSiteName,
                          SourceSiteNameFa = P.SourceSiteNameFa,
                          SourceSiteUrl = P.SourceSiteUrl,
                          Status = P.Status,
                          StatusAuthor = P.StatusAuthor,
                          ContentHTML = P.ContentHTML,
                          CommentCount = context.PostComments.Count(x => x.PostID_fk == P.PostID),
                          PostCategoryID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postCategoryID select _PB.PubBaseID).FirstOrDefault(),
                          PostFormatID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postFormatID select _PB.PubBaseID).FirstOrDefault(),
                          PostColID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == PostColID select _PB.PubBaseID).FirstOrDefault(),
                          PostCategoryName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postCategoryID select _PB.NameFa).FirstOrDefault(),
                          PostFormatName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postFormatID select _PB.NameFa).FirstOrDefault(),
                          PostColName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == PostColID select _PB.NameFa).FirstOrDefault(),

                      };
            return res;
        }

        public VM_PostManage Add(VM_PostManage vm)
        {
            var entity = new Post()
            {
                PostID = vm.PostID,
                Subject1 = vm.Subject1,
                SubSubject1_1 = vm.SubSubject1_1,
                SubSubject1_2 = vm.SubSubject1_2,
                ContentPost1_1 = vm.ContentPost1_1,
                ContentPost1_2 = vm.ContentPost1_2,
                ContentPost1_3 = vm.ContentPost1_3,
                ContentPost1_4 = vm.ContentPost1_4,
                ContentPost1_5 = vm.ContentPost1_5,
                Image1_1 = vm.Image1_1,
                Image1_2 = vm.Image1_2,
                Image1_3 = vm.Image1_3,
                Subject2 = vm.Subject2,
                SubSubject2_1 = vm.SubSubject2_1,
                SubSubject2_2 = vm.SubSubject2_2,
                ContentPost2_1 = vm.ContentPost2_1,
                ContentPost2_2 = vm.ContentPost2_2,
                ContentPost2_3 = vm.ContentPost2_3,
                ContentPost2_4 = vm.ContentPost2_4,
                ContentPost2_5 = vm.ContentPost2_5,
                Image2_1 = vm.Image2_1,
                Image2_2 = vm.Image2_2,
                Image2_3 = vm.Image2_3,
                Subject3 = vm.Subject3,
                SubSubject3_1 = vm.SubSubject3_1,
                SubSubject3_2 = vm.SubSubject3_2,
                ContentPost3_1 = vm.ContentPost3_1,
                ContentPost3_2 = vm.ContentPost3_2,
                ContentPost3_3 = vm.ContentPost3_3,
                ContentPost3_4 = vm.ContentPost3_4,
                ContentPost3_5 = vm.ContentPost3_5,
                Image3_1 = vm.Image3_1,
                Image3_2 = vm.Image3_2,
                Image3_3 = vm.Image3_3,
                Subject4 = vm.Subject4,
                SubSubject4_1 = vm.SubSubject4_1,
                SubSubject4_2 = vm.SubSubject4_2,
                ContentPost4_1 = vm.ContentPost4_1,
                ContentPost4_2 = vm.ContentPost4_2,
                ContentPost4_3 = vm.ContentPost4_3,
                ContentPost4_4 = vm.ContentPost4_4,
                ContentPost4_5 = vm.ContentPost4_5,
                Image4_1 = vm.Image4_1,
                Image4_2 = vm.Image4_2,
                Image4_3 = vm.Image4_3,
                Subject5 = vm.Subject5,
                SubSubject5_1 = vm.SubSubject5_1,
                SubSubject5_2 = vm.SubSubject5_2,
                ContentPost5_1 = vm.ContentPost5_1,
                ContentPost5_2 = vm.ContentPost5_2,
                ContentPost5_3 = vm.ContentPost5_3,
                ContentPost5_4 = vm.ContentPost5_4,
                ContentPost5_5 = vm.ContentPost5_5,
                Image5_1 = vm.Image5_1,
                Image5_2 = vm.Image5_2,
                Image5_3 = vm.Image5_3,
                Subject6 = vm.Subject6,
                SubSubject6_1 = vm.SubSubject6_1,
                SubSubject6_2 = vm.SubSubject6_2,
                ContentPost6_1 = vm.ContentPost6_1,
                ContentPost6_2 = vm.ContentPost6_2,
                ContentPost6_3 = vm.ContentPost6_3,
                ContentPost6_4 = vm.ContentPost6_4,
                ContentPost6_5 = vm.ContentPost6_5,
                Image6_1 = vm.Image6_1,
                Image6_2 = vm.Image6_2,
                Image6_3 = vm.Image6_3,
                Subject7 = vm.Subject7,
                SubSubject7_1 = vm.SubSubject7_1,
                SubSubject7_2 = vm.SubSubject7_2,
                ContentPost7_1 = vm.ContentPost7_1,
                ContentPost7_2 = vm.ContentPost7_2,
                ContentPost7_3 = vm.ContentPost7_3,
                ContentPost7_4 = vm.ContentPost7_3,
                ContentPost7_5 = vm.ContentPost7_5,
                Image7_1 = vm.Image7_1,
                Image7_2 = vm.Image7_2,
                Image7_3 = vm.Image7_3,
                Subject8 = vm.Subject8,
                SubSubject8_1 = vm.SubSubject8_1,
                SubSubject8_2 = vm.SubSubject8_2,
                ContentPost8_1 = vm.ContentPost8_1,
                ContentPost8_2 = vm.ContentPost8_2,
                ContentPost8_3 = vm.ContentPost8_3,
                ContentPost8_4 = vm.ContentPost8_4,
                ContentPost8_5 = vm.ContentPost8_5,
                Image8_1 = vm.Image8_1,
                Image8_2 = vm.Image8_2,
                Image8_3 = vm.Image8_3,
                Subject9 = vm.Subject9,
                SubSubject9_1 = vm.SubSubject9_1,
                SubSubject9_2 = vm.SubSubject9_2,
                ContentPost9_1 = vm.ContentPost9_1,
                ContentPost9_2 = vm.ContentPost9_2,
                ContentPost9_3 = vm.ContentPost9_3,
                ContentPost9_4 = vm.ContentPost9_4,
                ContentPost9_5 = vm.ContentPost9_5,
                Image9_1 = vm.Image9_1,
                Image9_2 = vm.Image9_2,
                Image9_3 = vm.Image9_3,
                Subject10 = vm.Subject10,
                SubSubject10_1 = vm.SubSubject10_1,
                SubSubject10_2 = vm.SubSubject10_2,
                ContentPost10_1 = vm.ContentPost10_1,
                ContentPost10_2 = vm.ContentPost10_2,
                ContentPost10_3 = vm.ContentPost10_3,
                ContentPost10_4 = vm.ContentPost10_4,
                ContentPost10_5 = vm.ContentPost10_5,
                Image10_1 = vm.Image10_1,
                Image10_2 = vm.Image10_2,
                Image10_3 = vm.Image10_3,
                Subject11 = vm.Subject11,
                SubSubject11_1 = vm.SubSubject11_1,
                SubSubject11_2 = vm.SubSubject11_2,
                ContentPost11_1 = vm.ContentPost11_1,
                ContentPost11_2 = vm.ContentPost11_2,
                ContentPost11_3 = vm.ContentPost11_3,
                ContentPost11_4 = vm.ContentPost11_4,
                ContentPost11_5 = vm.ContentPost11_5,
                Image11_1 = vm.Image11_1,
                Image11_2 = vm.Image11_2,
                Image11_3 = vm.Image11_3,
                Subject12 = vm.Subject12,
                SubSubject12_1 = vm.SubSubject12_1,
                SubSubject12_2 = vm.SubSubject12_2,
                ContentPost12_1 = vm.ContentPost12_1,
                ContentPost12_2 = vm.ContentPost12_2,
                ContentPost12_3 = vm.ContentPost12_3,
                ContentPost12_4 = vm.ContentPost12_4,
                ContentPost12_5 = vm.ContentPost12_5,
                Image12_1 = vm.Image12_1,
                Image12_2 = vm.Image12_2,
                Image12_3 = vm.Image12_3,
                Subject13 = vm.Subject13,
                SubSubject13_1 = vm.SubSubject13_1,
                SubSubject13_2 = vm.SubSubject13_2,
                ContentPost13_1 = vm.ContentPost13_1,
                ContentPost13_2 = vm.ContentPost13_2,
                ContentPost13_3 = vm.ContentPost13_3,
                ContentPost13_4 = vm.ContentPost13_4,
                ContentPost13_5 = vm.ContentPost13_5,
                Image13_1 = vm.Image13_1,
                Image13_2 = vm.Image13_2,
                Image13_3 = vm.Image13_3,
                Subject14 = vm.Subject14,
                SubSubject14_1 = vm.SubSubject14_1,
                SubSubject14_2 = vm.SubSubject14_2,
                ContentPost14_1 = vm.ContentPost14_1,
                ContentPost14_2 = vm.ContentPost14_2,
                ContentPost14_3 = vm.ContentPost14_3,
                ContentPost14_4 = vm.ContentPost14_4,
                ContentPost14_5 = vm.ContentPost14_5,
                Image14_1 = vm.Image14_1,
                Image14_2 = vm.Image14_2,
                Image14_3 = vm.Image14_3,
                Subject15 = vm.Subject15,
                SubSubject15_1 = vm.SubSubject15_1,
                SubSubject15_2 = vm.SubSubject15_2,
                ContentPost15_1 = vm.ContentPost15_1,
                ContentPost15_2 = vm.ContentPost15_2,
                ContentPost15_3 = vm.ContentPost15_3,
                ContentPost15_4 = vm.ContentPost15_4,
                ContentPost15_5 = vm.ContentPost15_5,
                Image15_1 = vm.Image15_1,
                Image15_2 = vm.Image15_2,
                Image15_3 = vm.Image15_3,
                Subject16 = vm.Subject16,
                SubSubject16_1 = vm.SubSubject16_1,
                SubSubject16_2 = vm.SubSubject16_2,
                ContentPost16_1 = vm.ContentPost16_1,
                ContentPost16_2 = vm.ContentPost16_2,
                ContentPost16_3 = vm.ContentPost16_3,
                ContentPost16_4 = vm.ContentPost16_4,
                ContentPost16_5 = vm.ContentPost16_5,
                Image16_1 = vm.Image16_1,
                Image16_2 = vm.Image16_2,
                Image16_3 = vm.Image16_3,
                Subject17 = vm.Subject17,
                SubSubject17_1 = vm.SubSubject17_1,
                SubSubject17_2 = vm.SubSubject17_2,
                ContentPost17_1 = vm.ContentPost17_1,
                ContentPost17_2 = vm.ContentPost17_2,
                ContentPost17_3 = vm.ContentPost17_3,
                ContentPost17_4 = vm.ContentPost17_4,
                ContentPost17_5 = vm.ContentPost17_5,
                Image17_1 = vm.Image17_1,
                Image17_2 = vm.Image17_2,
                Image17_3 = vm.Image17_3,
                Subject18 = vm.Subject18,
                SubSubject18_1 = vm.SubSubject18_1,
                SubSubject18_2 = vm.SubSubject18_2,
                ContentPost18_1 = vm.ContentPost18_1,
                ContentPost18_2 = vm.ContentPost18_2,
                ContentPost18_3 = vm.ContentPost18_3,
                ContentPost18_4 = vm.ContentPost18_4,
                ContentPost18_5 = vm.ContentPost18_5,
                Image18_1 = vm.Image18_1,
                Image18_2 = vm.Image18_2,
                Image18_3 = vm.Image18_3,
                Subject19 = vm.Subject19,
                SubSubject19_1 = vm.SubSubject19_1,
                SubSubject19_2 = vm.SubSubject19_2,
                ContentPost19_1 = vm.ContentPost19_1,
                ContentPost19_2 = vm.ContentPost19_2,
                ContentPost19_3 = vm.ContentPost19_3,
                ContentPost19_4 = vm.ContentPost19_4,
                ContentPost19_5 = vm.ContentPost19_5,
                Image19_1 = vm.Image19_1,
                Image19_2 = vm.Image19_2,
                Image19_3 = vm.Image19_3,
                Subject20 = vm.Subject20,
                SubSubject20_1 = vm.SubSubject20_1,
                SubSubject20_2 = vm.SubSubject20_2,
                ContentPost20_1 = vm.ContentPost20_1,
                ContentPost20_2 = vm.ContentPost20_2,
                ContentPost20_3 = vm.ContentPost20_3,
                ContentPost20_4 = vm.ContentPost20_4,
                ContentPost20_5 = vm.ContentPost20_5,
                Image20_1 = vm.Image20_1,
                Image20_2 = vm.Image20_2,
                Image20_3 = vm.Image20_3,
                Url = vm.Url,
                UrlMP3 = vm.UrlMP3,
                UrlVideo = vm.UrlVideo,
                Views = vm.Views,
                LikePost = vm.LikePost,
                DislikePost = vm.DislikePost,
                PublishCount = vm.PublishCount,
                BackgroundColor = vm.BackgroundColor == "" ? "#a94442" : vm.BackgroundColor,
                ModifyUserID = vm.ModifyUserID,
                ModifyDate = vm.ModifyDate,
                IsCreatedPost = vm.IsCreatedPost,
                Cat1 = vm.Cat1,
                Cat2 = vm.Cat2,
                Cat3 = vm.Cat3,
                Tag1 = vm.Tag1,
                Tag2 = vm.Tag2,
                Tag3 = vm.Tag3,
                Tag4 = vm.Tag4,
                Tag5 = vm.Tag5,
                Tag6 = vm.Tag6,
                Tag7 = vm.Tag7,
                Tag8 = vm.Tag8,
                Tag9 = vm.Tag9,
                Tag10 = vm.Tag10,
                ContentPost1_6 = vm.ContentPost1_6,
                ContentPost1_7 = vm.ContentPost1_7,
                SourceDateTimePost = vm.SourceDateTimePost,
                SourceFootCategory = vm.SourceFootCategory,
                SourceSiteName = vm.SourceSiteName,
                SourceSiteNameFa = vm.SourceSiteNameFa,
                SourceSiteUrl = vm.SourceSiteUrl,
                ContentHTML = vm.ContentHTML,
                ScriptAparat = vm.ScriptAparat
            };
            var res = context.Posts.Add(entity);
            context.SaveChanges();


            //طبقه بندی پست
            var enntiyCat = new PostAttribute()
            {
                AttributeID_fk = vm.PostCategoryID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyCat);

            //فرمت پست
            var enntiyFormat = new PostAttribute()
            {
                AttributeID_fk = vm.PostFormatID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyFormat);

            //ستون پست
            var enntiyCol = new PostAttribute()
            {
                AttributeID_fk = vm.PostColID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyCol);

            //ایجا یوزر پست ها
            Random r = new Random();
            context.UserPosts.Add(new UserPost()
            {
                ModifyDate = DateTime.Now,
                ModifyUserID = 1,
                PostID_fk = entity.PostID,
                UserID_fk = r.Next(1, 3),
            });

            context.SaveChanges();



            return Select(res.PostID);
        }

        public VM_PostManage Update(VM_PostManage vm)
        {
            //var entity = new Post()
            //{
            //    PostID = vm.PostID,
            //    Subject1 = vm.Subject1,
            //    SubSubject1_1 = vm.SubSubject1_1,
            //    SubSubject1_2 = vm.SubSubject1_2,
            //    ContentPost1_1 = vm.ContentPost1_1,
            //    ContentPost1_2 = vm.ContentPost1_2,
            //    ContentPost1_3 = vm.ContentPost1_3,
            //    ContentPost1_4 = vm.ContentPost1_4,
            //    ContentPost1_5 = vm.ContentPost1_5,
            //    Image1_1 = vm.Image1_1,
            //    Image1_2 = vm.Image1_2,
            //    Image1_3 = vm.Image1_3,
            //    Url = vm.Url,
            //    UrlMP3 = vm.UrlMP3,
            //    UrlVideo = vm.UrlVideo,
            //    Views = vm.Views,
            //    LikePost = vm.LikePost,
            //    DislikePost = vm.DislikePost,
            //    PublishCount = vm.PublishCount,
            //    BackgroundColor = vm.BackgroundColor == "" ? "#a94442" : vm.BackgroundColor,
            //    ModifyUserID = vm.ModifyUserID,
            //    ModifyDate = vm.ModifyDate,
            //    IsCreatedPost = vm.IsCreatedPost,
            //    Cat1 = vm.Cat1,
            //    Cat2 = vm.Cat2,
            //    Cat3 = vm.Cat3,
            //    Tag1 = vm.Tag1,
            //    Tag2 = vm.Tag2,
            //    Tag3 = vm.Tag3,
            //    Tag4 = vm.Tag4,
            //    Tag5 = vm.Tag5,
            //    SourceDateTimePost = vm.SourceDateTimePost,
            //    SourceFootCategory = vm.SourceFootCategory,
            //    SourceSiteName = vm.SourceSiteName,
            //    SourceSiteNameFa = vm.SourceSiteNameFa,
            //    SourceSiteUrl = vm.SourceSiteUrl,
            //    ContentHTML = vm.ContentHTML,
            //    ScriptAparat = vm.ScriptAparat
            //};

            var entity = context.Posts.First(p => p.PostID == vm.PostID);
            entity.Subject1 = vm.Subject1;
            entity.SubSubject1_1 = vm.SubSubject1_1;
            entity.SubSubject1_2 = vm.SubSubject1_2;
            entity.ContentPost1_1 = vm.ContentPost1_1;
            entity.ContentPost1_2 = vm.ContentPost1_2;
            entity.ContentPost1_3 = vm.ContentPost1_3;
            entity.ContentPost1_4 = vm.ContentPost1_4;
            entity.ContentPost1_5 = vm.ContentPost1_5;
            entity.Image1_1 = vm.Image1_1;
            entity.Image1_2 = vm.Image1_2;
            entity.Image1_3 = vm.Image1_3;
            entity.Url = vm.Url;
            entity.UrlMP3 = vm.UrlMP3;
            entity.UrlVideo = vm.UrlVideo;
            entity.Views = vm.Views;
            entity.LikePost = vm.LikePost;
            entity.DislikePost = vm.DislikePost;
            entity.PublishCount = vm.PublishCount;
            entity.BackgroundColor = vm.BackgroundColor == "" ? "#a94442" : vm.BackgroundColor;
            entity.ModifyUserID = vm.ModifyUserID;
            entity.ModifyDate = vm.ModifyDate;
            entity.IsCreatedPost = vm.IsCreatedPost;
            entity.Cat1 = vm.Cat1;
            entity.Cat2 = vm.Cat2;
            entity.Cat3 = vm.Cat3;
            entity.Tag1 = vm.Tag1;
            entity.Tag2 = vm.Tag2;
            entity.Tag3 = vm.Tag3;
            entity.Tag4 = vm.Tag4;
            entity.Tag5 = vm.Tag5;
            entity.SourceDateTimePost = vm.SourceDateTimePost;
            entity.SourceFootCategory = vm.SourceFootCategory;
            entity.SourceSiteName = vm.SourceSiteName;
            entity.SourceSiteNameFa = vm.SourceSiteNameFa;
            entity.SourceSiteUrl = vm.SourceSiteUrl;
            entity.ContentHTML = vm.ContentHTML;
            entity.ScriptAparat = vm.ScriptAparat;

            ////var res = context.Posts.Attach(entity);
            ////context.Entry(entity).State = EntityState.Modified;
            //context.SaveChanges();

            //حذف مشخصات پست
            var attrs = context.PostAttributes.Where(p => p.PostID_fk == vm.PostID).ToList();
            foreach (var item in attrs)
            {
                context.PostAttributes.Remove(item);
            }
            context.SaveChanges();

            ////////ایجاد مشخصات پشت////////
            //طبقه بندی پست
            var enntiyCat = new PostAttribute()
            {
                AttributeID_fk = vm.PostCategoryID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyCat);

            //فرمت پست
            var enntiyFormat = new PostAttribute()
            {
                AttributeID_fk = vm.PostFormatID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyFormat);

            //ستون پست
            var enntiyCol = new PostAttribute()
            {
                AttributeID_fk = vm.PostColID,
                PostID_fk = entity.PostID,
            };
            context.PostAttributes.Add(enntiyCol);


            var user = context.UserPosts.FirstOrDefault(p => p.PostID_fk == entity.PostID);
            if (user == null)
            {
                //ایجا یوزر پست ها
                Random r = new Random();
                context.UserPosts.Add(new UserPost()
                {
                    ModifyDate = DateTime.Now,
                    ModifyUserID = 1,
                    PostID_fk = entity.PostID,
                    UserID_fk = r.Next(1, 3),
                });
            }

            context.SaveChanges();

            return Select(vm.PostID);
        }

        public VM_PostManage Delete(int id)
        {
            var entity = context.Posts.FirstOrDefault(p => p.PostID == id);
            if (entity != null)
            {
                context.Posts.Remove(entity);
                context.SaveChanges();
            }

            return new VM_PostManage();
        }

        public VM_PostManage Select(int id)
        {
            var postCategoryID = context.PubBases.First(p => p.NameEn == "PostCategory").PubBaseID;
            var postFormatID = context.PubBases.First(p => p.NameEn == "PostFormat").PubBaseID;
            var PostColID = context.PubBases.First(p => p.NameEn == "PostCol").PubBaseID;

            var res = from P in context.Posts
                      join UP in context.UserPosts on P.PostID equals UP.PostID_fk
                      join U in context.Users on UP.UserID_fk equals U.UserID
                      select new VM_PostManage
                      {
                          PostID = P.PostID,
                          Subject1 = P.Subject1,
                          SubSubject1_1 = P.SubSubject1_1,
                          SubSubject1_2 = P.SubSubject1_2,
                          ContentPost1_1 = P.ContentPost1_1,
                          ContentPost1_2 = P.ContentPost1_2,
                          ContentPost1_3 = P.ContentPost1_3,
                          ContentPost1_4 = P.ContentPost1_4,
                          ContentPost1_5 = P.ContentPost1_5,
                          Image1_1 = P.Image1_1,
                          Image1_2 = P.Image1_2,
                          Image1_3 = P.Image1_3,
                          Url = P.Url,
                          UrlMP3 = P.UrlMP3,
                          UrlVideo = P.UrlVideo,
                          Views = P.Views,
                          LikePost = P.LikePost,
                          DislikePost = P.DislikePost,
                          PublishCount = P.PublishCount,
                          BackgroundColor = P.BackgroundColor,
                          ModifyUserID = P.ModifyUserID,
                          ModifyDate = P.ModifyDate,
                          IsCreatedPost = P.IsCreatedPost,
                          Cat1 = P.Cat1,
                          Cat2 = P.Cat2,
                          Cat3 = P.Cat3,
                          Tag1 = P.Tag1,
                          Tag2 = P.Tag2,
                          Tag3 = P.Tag3,
                          Tag4 = P.Tag4,
                          Tag5 = P.Tag5,
                          FootCategory = P.SourceFootCategory,
                          DateTimePost = P.SourceDateTimePost,
                          SourceDateTimePost = P.SourceDateTimePost,
                          SourceFootCategory = P.SourceFootCategory,
                          SourceSiteName = P.SourceSiteName,
                          SourceSiteNameFa = P.SourceSiteNameFa,
                          SourceSiteUrl = P.SourceSiteUrl,
                          Status = P.Status,
                          StatusAuthor = P.StatusAuthor,
                          ContentHTML = P.ContentHTML,
                          CommentCount = context.PostComments.Count(x => x.PostID_fk == P.PostID),
                          PostCategoryID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postCategoryID select _PB.PubBaseID).FirstOrDefault(),
                          PostFormatID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postFormatID select _PB.PubBaseID).FirstOrDefault(),
                          PostColID = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == PostColID select _PB.PubBaseID).FirstOrDefault(),
                          PostCategoryName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postCategoryID select _PB.NameFa).FirstOrDefault(),
                          PostFormatName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == postFormatID select _PB.NameFa).FirstOrDefault(),
                          PostColName = (from _P in context.Posts join _PA in context.PostAttributes on _P.PostID equals _PA.PostID_fk join _PB in context.PubBases on _PA.AttributeID_fk equals _PB.PubBaseID where _P.PostID == P.PostID && _PB.ParentID == PostColID select _PB.NameFa).FirstOrDefault(),

                      };
            return res.FirstOrDefault();
        }

        public IQueryable<VM_PostManage> SelectAll(string condition)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePublishCount(int id)
        {
            var res = context.Posts.FirstOrDefault(p => p.PostID == id);
            if (res == null)
                return false;

            res.PublishCount = (res.PublishCount == null ? 0 : res.PublishCount) + 1;
            context.SaveChanges();
            return true;
        }


        /// <summary>
        /// فرمت پست به لينک تغيير پيدا مي کند
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public bool CreateNewFormatPost(int postid, string formatType)
        {
            var linkID = context.PubBases.First(p => p.NameEn == formatType).PubBaseID;

            var postFormatID = context.PubBases.First(p => p.NameEn == "PostFormat").PubBaseID;
            var FormatIDs = context.PubBases.Where(p => p.ParentID == postFormatID).Select(p => p.PubBaseID).ToList();

            var res = context.PostAttributes.Where(p => p.PostID_fk == postid && FormatIDs.Contains(p.AttributeID_fk)).ToList();
            if (res != null)
            {
                context.PostAttributes.RemoveRange(res);

                context.PostAttributes.Add(new PostAttribute()
                {
                    PostID_fk = postid,
                    AttributeID_fk = linkID,
                });

                context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}