using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace aCudaResearch.Settings
{
    interface ISettingsBuilder
    {
        /// <summary>
        /// Build settings.
        /// </summary>
        /// <returns><see link="ExecutionSettings"/></returns>
        ExecutionSettings Build();
    }
}
