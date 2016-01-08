using GalaSoft.MvvmLight.Messaging;
using Piazza.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Piazza.Messages
{
    public class FetchPostMessage : MessageBase
    {
        public string Cookies { get; set; }
        public RegisteredClass SelectedClass { get; set; }
        public ClassFeed SelectedFeedItem { get; set; }
    }
}
