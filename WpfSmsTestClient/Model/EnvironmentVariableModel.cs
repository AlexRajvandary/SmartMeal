using System.ComponentModel;

namespace WpfSmsTestClient.Model
{
    public class EnvironmentVariableModel
    {
        [Description("Название")]
        public string Name { get; set; }

        [Description("Значение")]
        public string Value { get; set; }

        [Description("Комментарий")]
        public string Comment { get; set; }
    }
}
