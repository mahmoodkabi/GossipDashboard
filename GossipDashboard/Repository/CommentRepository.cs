using GossipDashboard.Models;
using GossipDashboard.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Repository
{
    public class CommentRepository : IRepository<VM_Comment>
    {
        private GossipSiteEntities context = new GossipSiteEntities();
        public VM_Comment Add(VM_Comment vm)
        {
            BlackListRepository blackListRepo = new BlackListRepository();

            //پست هايي که شامل بلک ليست هستند ناديده گرفته مي شود
            var blackList = blackListRepo.SelectByParentName("Political").Select(p => p.Name).ToList();
            blackList.AddRange(blackListRepo.SelectByParentName("Insulting").Select(p => p.Name).ToList());

            //تطبیق پست ها با لیست سیاه
            var isBlack = CheckBlackList(blackList, vm);
            if (isBlack)
            {
                return new VM_Comment();
            }

            var entity = new PostComment()
            {
                FullName = vm.FullName,
                IPAddress = vm.IPAddress,
                Datetime = vm.Datetime,
                Comment = vm.Comment,
                DislikeComment = vm.DislikeComment,
                LikeComment = vm.LikeComment,
                PostID_fk = vm.PostID_fk,
                UserID = vm.UserID,
            };

            context.PostComments.Add(entity);
            context.SaveChanges();

            vm.PostCommentID = entity.PostCommentID;
            return vm;
        }

        public VM_Comment Delete(int id)
        {
            throw new NotImplementedException();
        }

        public VM_Comment Select(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<VM_Comment> SelectAll(string condition)
        {
            var res = context.PostComments.Select(p => new VM_Comment()
            {
                FullName = p.FullName,
                IPAddress = p.IPAddress,
                Datetime = p.Datetime,
                Comment = p.Comment,
                DislikeComment = p.DislikeComment,
                LikeComment = p.LikeComment,
                PostID_fk = p.PostID_fk,
                UserID = p.UserID,
                PostCommentID = p.PostCommentID,
                TotalComment = context.PostComments.Where(pc => pc.PostID_fk == p.PostID_fk).Count(),
            });
            return res;
        }

        public VM_Comment Update(VM_Comment vm)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// این متد پست ها را با لیست سیاه چک می کند و پست های نامناسب را علامت می گذارد
        /// </summary>
        /// <param name="blackList"></param>
        /// <param name="item"></param>
        private bool CheckBlackList(List<string> blackList, VM_Comment item)
        {
            foreach (string itemBlackList in blackList)
            {
                if (item.Comment.Contains(itemBlackList) || item.FullName.Contains(itemBlackList))
                {
                    return true; ;
                }
            }

            return false;
        }

    }
}