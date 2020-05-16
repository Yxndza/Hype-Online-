using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HypestoreFinal.Models;
using System.ComponentModel.DataAnnotations;

namespace HypestoreFinal.ViewModels
{
    public class HypeProjectReport
    {
        private List<Subscription> subscriptions;

        public HypeProjectReport(List<Subscription> subscriptions)
        {
            this.subscriptions = subscriptions;
            Count = subscriptions.Count();
        }

        //Number of Entries
        public int Count { get; set; }

        //Each value for "How They Heard About Us"
        //Advert
        public int AdvertCount => subscriptions
            .Where(s => s.Source == Source.Advert).Count();

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public double? AdvertRatio => (Count > 0) ?
            (double)AdvertCount / (double)Count : 0;

        //Word of Mouth
        public int WordOfMouthCount => subscriptions
            .Where(s => s.Source == Source.WordOfMouth).Count();
        [DisplayFormat(DataFormatString = "{0:P2}")]

        public double? WordOfMounthRatio => (Count > 0) ?
            (double)WordOfMouthCount / (double)Count : 0;

        //Other
        public int OtherCount => subscriptions
            .Where(s => s.Source == Source.Other).Count();
        [DisplayFormat(DataFormatString = "{0:P2}")]

        public double? OtherRatio => (Count > 0) ?
            (double)OtherCount / (double)Count : 0;                

        //Reason For Signing Up
        public int ReasonEnteredCount => subscriptions.Where(s => !string.IsNullOrWhiteSpace(s.Reason)).Count();

        //Answer entered
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public double? ReasonEnteredRatio => (ReasonEnteredCount > 0) ? 
            (double)ReasonEnteredCount / (double)Count : 0;

        //Answer not entered
        public int ReasonNotEnteredCount => subscriptions.Where(s => string.IsNullOrWhiteSpace(s.Reason)).Count();

        [DisplayFormat(DataFormatString = "{0:P2}")]
        public double? ReasonNotEnteredRatio => (ReasonNotEnteredCount > 0) ?
            (double)ReasonNotEnteredCount / (double)Count : 0;
    }
}