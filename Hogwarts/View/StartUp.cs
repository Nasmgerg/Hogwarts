using Hogwarts.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Hogwarts.View
{
    public partial class StartUp : Form
    {
        private readonly IServiceProvider serviceProvider;

        public StartUp(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
            InitializeComponent();
        }

        private void LoadWizards(object sender, EventArgs e)
        {
            UpdateGridView();
        }

        private void ListWizards(object sender, EventArgs e)
        {
            UpdateGridView();
        }

        private void UpdateGridView()
        {
            var context = serviceProvider.GetRequiredService<WizardContext>();

            wizardGridView.DataSource = null;
            wizardGridView.DataSource = context.Wizards
                .OrderBy(w => w.Role)
                .ThenBy(w => w.Class)
                .ThenBy(w => w.House)
                .Select(w => new { w.Name, w.Role, w.House, w.Patronus, w.Class })
                .ToList();
        }
        
        private void AddWizard(object sender, EventArgs e)
        {
            using var window = serviceProvider.GetRequiredService<Add>();
            window.ShowDialog();

            if (window.DialogResult == DialogResult.OK)
                UpdateGridView();
        }
    }
}