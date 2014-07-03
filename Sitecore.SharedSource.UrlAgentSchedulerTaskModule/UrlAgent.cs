namespace Sitecore.SharedSource.UrlAgentSchedulerTaskModule
{
    using System;
    using System.Linq;
    using Data.Fields;
    using Data.Items;
    using Diagnostics;
    using Tasks;
    using Web;

    /// <summary>
    /// Represents a Url Agent Scheduler Task Module.
    /// 
    /// </summary>
    public class UrlAgent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="commandItem"></param>
        /// <param name="scheduleItem"></param>
        public void Run(Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            if (items != null)
            {
                foreach (Item item in items)
                {
                    MultilistField referenceField = item.Fields["Urls"];

                    if (referenceField != null)
                    {
                        Item[] targetUrlItems = referenceField.GetItems().Where(urlItem => urlItem != null).ToArray();

                        foreach (Item targetUrlItem in targetUrlItems)
                        {
                            string url = targetUrlItem["Url"];
                            RunTheUrl(url);
                        }


                    }
                }
            }

        }

        private void RunTheUrl(string url)
        {

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    string fullUrl = WebUtil.GetFullUrl(url.Trim());

                    Log.Info("Scheduling.UrlAgent started. Url: " + fullUrl, this);
                    Log.Info("Scheduling.UrlAgent done (received: " + WebUtil.ExecuteWebPage(fullUrl).Length + " bytes)", this);
                }
                catch (Exception ex)
                {
                    Log.Error("Exception in UrlAgent (url: " + url + ")", ex, this);
                }

            }
            else
            {
                Log.Warn("UrlAgent is empty ", this);

            }

        }
    }
}
