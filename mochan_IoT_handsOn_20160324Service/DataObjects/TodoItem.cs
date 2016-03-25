using Microsoft.Azure.Mobile.Server;

namespace mochan_IoT_handsOn_20160324Service.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }
    }
}