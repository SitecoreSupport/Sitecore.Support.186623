namespace Sitecore.Support.Pipelines.Save
{
    using Sitecore.Data;
    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.Save;

    /// <summary>
    /// Check HTML Entities
    /// </summary>
    public class CheckHtmlEntities
    {
        /// <summary>
		/// Runs the processor.
		/// </summary>
		/// <param name="args">The arguments.</param>
		public void Process(SaveArgs args)
        {
            // Fix bug 186623
            // Double encode localize parameters validation message
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.Items, "args.Items");
            SaveArgs.SaveItem[] items = args.Items;
            for (int i = 0; i < items.Length; i++)
            {
                SaveArgs.SaveItem saveItem = items[i];
                SaveArgs.SaveField[] fields = saveItem.Fields;
                for (int j = 0; j < fields.Length; j++)
                {
                    SaveArgs.SaveField saveField = fields[j];
                    if (saveField.ID == new ID("{26CDC7C2-5307-4591-A7F9-5C034A05630A}"))
                    {
                        string prefixValidator = "<PredefinedValidatorTextMessage>";
                        string postfixValidator = "</PredefinedValidatorTextMessage>";
                        int startIndex = saveField.Value.IndexOf(prefixValidator);
                        int endIndex = saveField.Value.IndexOf(postfixValidator);
                        if (startIndex > -1 && endIndex > -1)
                        {
                            string othersMessage = saveField.Value.Substring(0, startIndex);
                            string validationMessage = saveField.Value.Substring(startIndex + 32, endIndex - startIndex - 32);                            
                            validationMessage = validationMessage.Replace("<", "&amp;lt;").Replace(">", "&amp;gt;").Replace("&lt;", "&amp;lt;").Replace("&gt;", "&amp;gt;");
                            saveField.Value = othersMessage + prefixValidator + validationMessage + postfixValidator;
                        }
                    }
                }
            }
        }
    }
}
