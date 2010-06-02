using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch.FpGrowth.Data.MsWeb
{
    /// <summary>
    /// Describes entry from the MsWeb data.
    /// </summary>
    class Element
    {
        /// <summary>
        /// Page title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Page URL address.
        /// </summary>
        public string Url { get; set; }
    }
}
