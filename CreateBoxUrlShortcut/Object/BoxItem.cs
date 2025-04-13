using System;
using System.Collections.Generic;

namespace filet_belated_8t.Tools.CreateBoxUrlShortcut.Object
{
    public class BoxItem
    {
        public string BoxId { get; set; }
        public int ItemType { get; set; }
        public string ParentItemId { get; set; }
        public string Name { get; set; }
        public string SortName { get; set; }
        public string OwnerId { get; set; }
        public string Checksum { get; set; }
        public int? Size { get; set; }
        public string LockId { get; set; }
        public string LockOwnerId { get; set; }
        public int? ContentCreatedAt { get; set; }
        public int? ContentUpdatedAt { get; set; }
        public string VersionId { get; set; }
        public string LockAppType { get; set; }

        public static BoxItem FromRowDictionaly(Dictionary<string, object> recode)
        {
            BoxItem boxItem = new BoxItem();

            foreach (string key in recode.Keys)
            {
                switch (key)
                {
                    case "box_id":
                        boxItem.BoxId = (string)recode[key];
                        break;
                    case "item_type":
                        boxItem.ItemType = (int)recode[key];
                        break;
                    case "parent_item_id":
                        boxItem.ParentItemId = (string)recode[key];
                        break;
                    case "name":
                        boxItem.Name = (string)recode[key];
                        break;
                    case "sort_name":
                        boxItem.SortName = (string)recode[key];
                        break;
                    case "owner_id":
                        boxItem.OwnerId = (string)recode[key];
                        break;
                    case "checksum":
                        boxItem.Checksum = (string)recode[key];
                        break;
                    case "size":
                        boxItem.Size = (int?)recode[key];
                        break;
                    case "lock_id":
                        boxItem.LockId = (string)recode[key];
                        break;
                    case "lock_owner_id":
                        boxItem.LockOwnerId = (string)recode[key];
                        break;
                    case "content_created_at":
                        boxItem.ContentCreatedAt = (int?)recode[key];
                        break;
                    case "content_updated_at":
                        boxItem.ContentUpdatedAt = (int?)recode[key];
                        break;
                    case "version_id":
                        boxItem.VersionId = (string)recode[key];
                        break;
                    case "lock_app_type":
                        boxItem.LockAppType = (string)recode[key];
                        break;
                }
            }

            return boxItem;
        }

        public static BoxItem[] FromRowDictionaly(Dictionary<string, object>[] recodes)
        {
            List<BoxItem> itemBoxList = new List<BoxItem>();

            foreach (Dictionary<string, object> recode in recodes)
            {
                itemBoxList.Add(FromRowDictionaly(recode));
            }

           return itemBoxList.ToArray();
        }
    }
}
