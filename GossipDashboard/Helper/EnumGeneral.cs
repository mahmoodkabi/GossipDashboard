using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GossipDashboard.Helper
{
    public enum PostType
    {
        Quiz = 1,
        Cute = 2
    }

    public enum PlaceInMainPage
    {
        //وسط و بالای صفحه اصلی
        MiddleIndex = 1,

        //آخرین رویدادها
        LastEvent = 2,

        //مطالب جالب
        InterestingStuff = 3,

        // قسمتی که عکس در سمت راست و توضیجات در سمت راست دارد-- زیر مطالب جالب
        BloglistDefault = 4,

        //اسلایدر پایینی
        SliderBottom = 5,

        //اسلایدر بالایی
        SliderTop = 6,

        //بیشترین بازدید
        Mostviewed = 7,

        //محبوب
        Popular = 8,

        //آخرين خبرها
        LaseNews = 9,

        //استیکر بالایی
        StickerTop  = 10
    }
}