using Hogwarts.Context;
using Hogwarts.Model;
using Microsoft.Extensions.DependencyInjection;

namespace Hogwarts.View
{
    public partial class Add : Form
    {
        private readonly IServiceProvider serviceProvider;

        public Add(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
            InitializeComponent();

            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleComboBox.Items.AddRange(new string[] { "diák", "oktató", "egyéb" });
            roleComboBox.SelectedIndex = 0;

            houseComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            houseComboBox.Items.AddRange(new string[] { "Griffendél", "Mardekár", "Hollóhát", "Hugrabug" });
        }

        private void Save(object sender, EventArgs e)
        {
            errorProvider.Clear();

            string name = nameTextBox.Text.Trim();
            string? role = roleComboBox.SelectedItem?.ToString();
            string? house = houseComboBox.SelectedItem?.ToString();
            string patronus = patronusTextBox.Text.Trim();
            int wizardClass = (int)classUpDown.Value;

            bool isValid = true;

            if (name.Length == 0)
            {
                errorProvider.SetError(nameTextBox, "A név megadása kötelező!");
                isValid = false;
            }

            if (role == null)
            {
                errorProvider.SetError(roleComboBox, "A szerepkör megadása kötelező!");
                isValid = false;
            }

            if (role == "diák")
            {
                if (house == null)
                {
                    errorProvider.SetError(houseComboBox, "Diáknál a ház kötelező!");
                    isValid = false;
                }

                if (wizardClass < 1 || wizardClass > 7)
                {
                    errorProvider.SetError(classUpDown, "Az osztály 1 és 7 között legyen!");
                    isValid = false;
                }
            }

            if (!isValid)
                return;

            var wizard = new Wizard
            {
                Name = name,
                Role = role,
                House = house,
                Patronus = patronus.Length > 0 ? patronus : null,
                Class = role == "diák" ? wizardClass : 0
            };

            var context = serviceProvider.GetRequiredService<WizardContext>();
            context.Wizards.Add(wizard);
            context.SaveChanges();

            DialogResult = DialogResult.OK;
        }

        private void Cancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}