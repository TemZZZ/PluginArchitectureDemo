using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfLibrary;

namespace Plugin1
{
    internal class TemplateVM1 : VMBase
    {
        private string _description = Properties.Strings.Plugin1Strings.TemplateVM1Description;
        private int _stageCount = 3;

        public string Description
        {
            get => _description;
            set => SetValue(ref _description, value);
        }

        public int StageCount
        {
            get => _stageCount;
            set => SetValue(ref _stageCount, value);
        }
    }
}