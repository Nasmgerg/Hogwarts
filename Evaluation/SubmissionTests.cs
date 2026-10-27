using Hogwarts.Context;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EvaluatorTests
{
    public class SubmissionTests
    {
        private const string TargetAssembly = "Hogwarts";
        private const string DbFileName = "wizards.db";
        private const string DbPath = $"../../../../{TargetAssembly}/DB/{DbFileName}";

        private static Assembly GetCompiledAssembly()
        {
            try
            {
                return Assembly.Load(TargetAssembly);
            }
            catch (Exception)
            {
                Assert.Fail($"[Error] Build failed, therefore, could not load assembly '{TargetAssembly}.dll'");
                return null!;
            }
        }

        [Fact]
        public void Test1_ModelClassExists_WithCorrectProperties()
        {
            var assembly = GetCompiledAssembly();
            var modelType = assembly.GetType($"{TargetAssembly}.Model.Wizard");

            Assert.True(modelType != null,
                $"[Model] Expected model class '{TargetAssembly}.Model.Wizard' was not found. Check namespace and class name.");

            var idProp = modelType!.GetProperty("ID");
            var nameProp = modelType!.GetProperty("Name");
            var roleProp = modelType!.GetProperty("Role");
            var houseProp = modelType!.GetProperty("House");
            var patronusProp = modelType!.GetProperty("Patronus");
            var classProp = modelType!.GetProperty("Class");

            Assert.Multiple(
                () => Assert.True(idProp != null, "[Model] Property 'ID' is missing from the 'Wizard' model class."),
                () =>
                {
                    if (idProp != null)
                        Assert.True(idProp!.PropertyType == typeof(int),
                            $"[Model] Property 'ID' in 'Wizard' must be of type 'int', but found '{idProp.PropertyType.Name}'.");
                },
                () => Assert.True(nameProp != null, "[Model] Property 'Name' is missing from the 'Wizard' model class."),
                () =>
                {
                    Assert.True(nameProp!.PropertyType == typeof(string),
                        $"[Model] Property 'Name' in 'Wizard' must be of type 'string', but found '{nameProp.PropertyType.Name}'.");
                },
                () => Assert.True(roleProp != null, "[Model] Property 'Role' is missing from the 'Wizard' model class."),
                () =>
                {
                    Assert.True(roleProp!.PropertyType == typeof(string),
                        $"[Model] Property 'Role' in 'Wizard' must be of type 'string', but found '{roleProp.PropertyType.Name}'.");
                },
                () => Assert.True(houseProp != null, "[Model] Property 'House' is missing from the 'Wizard' model class."),
                () =>
                {
                    Assert.True(houseProp!.PropertyType == typeof(string),
                       $"[Model] Property 'House' in 'Wizard' must be of type 'string', but found '{houseProp.PropertyType.Name}'.");
                },
                () => Assert.True(patronusProp != null, "[Model] Property 'Patronus' is missing from the 'Wizard' model class."),
                () =>
                {
                    Assert.True(patronusProp!.PropertyType == typeof(string),
                     $"[Model] Property 'Patronus' in 'Wizard' must be of type 'string', but found '{patronusProp.PropertyType.Name}'.");
                },
                () => Assert.True(classProp != null, "[Model] Property 'Class' is missing from the 'Wizard' model class."),
                () =>
                {
                    Assert.True(classProp!.PropertyType == typeof(int),
                        $"[Model] Property 'Class' in 'Wizard' must be of type 'int', but found '{classProp.PropertyType.Name}'.");
                }

            );
        }

        [Fact]
        public void Test2_DbContextExists_WithDbSet()
        {
            var assembly = GetCompiledAssembly();
            var contextType = assembly.GetType($"{TargetAssembly}.Context.WizardContext");
            var modelType = assembly.GetType($"{TargetAssembly}.Model.Wizard");

            Assert.True(contextType != null, $"[DbContext] DbContext class '{TargetAssembly}.Context.WizardContext' was not found.");
            Assert.True(modelType != null, "[DbContext] Cannot verify DbSet because 'Wizard' model class is missing.");

            var dbSetProp = contextType!.GetProperty("Wizards");
            var expectedType = typeof(DbSet<>).MakeGenericType(modelType!);

            Assert.Multiple(
                () => Assert.True(typeof(DbContext).IsAssignableFrom(contextType),
                    $"[DbContext] '{TargetAssembly}.Context.WizardContext' does not inherit from Microsoft.EntityFrameworkCore.DbContext."),
                () => Assert.True(dbSetProp != null, "[DbContext] Property 'Wizards' (DbSet<Wizard>) is missing from WizardContext."),
                () => Assert.True(dbSetProp!.PropertyType == expectedType,
                    $"[DbContext] Property 'Wizards' in WizardContext should be 'DbSet<Wizard>', but found '{dbSetProp.PropertyType.Name}'.")
            );
        }

        [Fact]
        public void Test3_MigrationsApplied_DatabaseExists()
        {
            Assert.True(File.Exists(DbPath), "[DB] Database file was not found. Migrations not run or wrong file name.");

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();

            var historyCommand = connection.CreateCommand();
            historyCommand.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='__EFMigrationsHistory'";
            var historyCount = (long)historyCommand.ExecuteScalar();

            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Wizards'";
            var tableCount = (long)tableCommand.ExecuteScalar();

            Assert.Multiple(
                () => Assert.True(historyCount > 0, "[DB] EF Migrations History table missing."),
                () => Assert.True(tableCount > 0, "[DB] Wizards table missing from database.")
            );
        }

        [Fact]
        public void Test4_FormUsesDependencyInjection()
        {
            var assembly = GetCompiledAssembly();
            var startUpForm = assembly.GetType($"{TargetAssembly}.View.StartUp");

            Assert.True(startUpForm != null, $"[DI] Form '{TargetAssembly}.View.StartUp' was not found.");

            var constructors = startUpForm!.GetConstructors();
            var startUpConstructor = constructors.FirstOrDefault(c =>
                c.GetParameters().Length == 1 &&
                c.GetParameters()[0].ParameterType == typeof(IServiceProvider));

            Assert.True(startUpConstructor != null,
               "[DI] The 'StartUp' form does not have a constructor that accepts 'IServiceProvider'. You should use Dependency Injection.");

            var addForm = assembly.GetType($"{TargetAssembly}.View.Add");
            Assert.True(addForm != null, $"[DI] Form '{TargetAssembly}.View.Add' was not found.");

            constructors = startUpForm!.GetConstructors();

            var addConstructor = constructors.FirstOrDefault(c =>
                c.GetParameters().Length == 1 &&
                c.GetParameters()[0].ParameterType == typeof(IServiceProvider));

            Assert.True(addConstructor != null,
                "[DI] The 'Add' form does not have a constructor that accepts 'IServiceProvider'. You should use Dependency Injection.");
        }

        private Form CreateAddForm()
        {
            var assembly = GetCompiledAssembly();
            var contextType = assembly.GetType($"{TargetAssembly}.Context.WizardContext");
            var formType = assembly.GetType($"{TargetAssembly}.View.Add");

            Assert.True(formType != null, $"[Add] Form '{TargetAssembly}.View.Add' was not found.");

            var optionsBuilder = new DbContextOptionsBuilder<WizardContext>();
            optionsBuilder.UseSqlite($"Data Source={DbPath}");

            var context = Activator.CreateInstance(contextType!, optionsBuilder.Options);
            var services = new ServiceCollection();
            services.AddSingleton(contextType!, context!);

            var serviceProvider = services.BuildServiceProvider();
            var form = (Form)Activator.CreateInstance(formType!, serviceProvider)!;

            return form;
        }


        // Structure Check
        [StaFact]
        public void Test5_AddForm_Structure()
        {
            var form = CreateAddForm();

            var nameTextBox = (TextBox)form.Controls.Find("nameTextBox", true).FirstOrDefault();
            var roleComboBox = (ComboBox)form.Controls.Find("roleComboBox", true).FirstOrDefault();
            var houseComboBox = (ComboBox)form.Controls.Find("houseComboBox", true).FirstOrDefault();
            var patronusTextBox = (TextBox)form.Controls.Find("patronusTextBox", true).FirstOrDefault();
            var classUpDown = (NumericUpDown)form.Controls.Find("classUpDown", true).FirstOrDefault();
            var addButton = (Button)form.Controls.Find("addButton", true).FirstOrDefault();
            var cancelButton = (Button)form.Controls.Find("cancelButton", true).FirstOrDefault();

            var assembly = GetCompiledAssembly();
            var formType = assembly.GetType($"{TargetAssembly}.View.Add");

            var fieldInfo = formType.GetField("errorProvider", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            Assert.Multiple(
                () => Assert.True(nameTextBox != null, "[Add] 'nameTexBox' was not found on Add Form."),
                () => Assert.True(roleComboBox != null, "[Add] 'roleComboBox' was not found on Add Form."),
                () => Assert.True(houseComboBox != null, "[Add] 'houseComboBox' was not found on Add Form."),
                () => Assert.True(patronusTextBox != null, "[Add] 'patronusTextBox' was not found on Add Form."),
                () => Assert.True(classUpDown != null, "[Add] 'classUpDown' was not found on Add Form."),
                () => Assert.True(addButton != null, "[Add] 'addButton' was not found on Add Form."),
                () => Assert.True(cancelButton != null, "[Add] 'cancelButton' was not found on Add Form."),
                () => Assert.True(fieldInfo != null, "[Add] 'errorProvider' was not found on Add Form.")


            );

            var errorProvider = (ErrorProvider?)fieldInfo!.GetValue(form);

            Assert.True(roleComboBox.DropDownStyle == ComboBoxStyle.DropDownList, "[Add] 'roleComboBox' DropDownStyle is not DropDownList.");
            Assert.True(houseComboBox.DropDownStyle == ComboBoxStyle.DropDownList, "[Add] 'houseComboBox' DropDownStyle is not DropDownList.");


            var requiredItems = new[] { "Griffendél", "Mardekár", "Hollóhát", "Hugrabug" };
            var actualItems = houseComboBox!.Items.Cast<object>()
                .Select(item => item.ToString())
                .ToList();

            Assert.Multiple(
                () => Assert.True(actualItems.Count >= requiredItems.Length,
                    $"[Add] houseComboBox: Expected at least {requiredItems.Length} items, but found {actualItems.Count}."),

                () =>
                {
                    foreach (var expectedItem in requiredItems)
                    {
                        Assert.True(actualItems.Contains(expectedItem),
                            $"[Add] Required option '{expectedItem}' was missing from 'houseComboBox'.");
                    }
                }
            );

            requiredItems = new[] { "diák", "oktató", "egyéb" };
            actualItems = roleComboBox!.Items.Cast<object>()
                .Select(item => item.ToString())
                .ToList();

            Assert.Multiple(
                () => Assert.True(actualItems.Count >= requiredItems.Length,
                    $"[Add] roleComboBox: Expected at least {requiredItems.Length} items, but found {actualItems.Count}."),

                () =>
                {
                    foreach (var expectedItem in requiredItems)
                    {
                        Assert.True(actualItems.Contains(expectedItem),
                            $"[Add] Required option '{expectedItem}' was missing from 'roleComboBox'.");
                    }
                }
            );
        }

        // Kind add: Everything is filled and should be OK.
        [StaFact]
        public void Test6_AddForm_InsertsRecordToDatabase()
        {
            var form = CreateAddForm();

            var nameTextBox = (TextBox)form.Controls.Find("nameTextBox", true).FirstOrDefault();
            var roleComboBox = (ComboBox)form.Controls.Find("roleComboBox", true).FirstOrDefault();
            var houseComboBox = (ComboBox)form.Controls.Find("houseComboBox", true).FirstOrDefault();
            var patronusTextBox = (TextBox)form.Controls.Find("patronusTextBox", true).FirstOrDefault();
            var classUpDown = (NumericUpDown)form.Controls.Find("classUpDown", true).FirstOrDefault();
            var addButton = (Button)form.Controls.Find("addButton", true).FirstOrDefault();
            var cancelButton = (Button)form.Controls.Find("cancelButton", true).FirstOrDefault();

            var assembly = GetCompiledAssembly();
            var formType = assembly.GetType($"{TargetAssembly}.View.Add");

            var fieldInfo = formType.GetField("errorProvider", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var errorProvider = (ErrorProvider?)fieldInfo!.GetValue(form);

            nameTextBox.Text = "Ron Weasley";
            roleComboBox.SelectedItem = "diák";
            houseComboBox.SelectedItem = "Griffendél";
            patronusTextBox.Text = "Jack Russell-terrier";
            classUpDown.Value = 4;

            form.Show();
            addButton.PerformClick();

            Assert.True(form.DialogResult == DialogResult.OK, "[DB] DialogResult should be OK");
            Assert.False(errorProvider!.HasErrors, "[DB] errorProvider should not have errors");

            Assert.True(File.Exists(DbPath), "[DB] Database file was not found. Migrations not run or wrong file name.");

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Class FROM Wizards WHERE Name = 'Ron Weasley'";
            var classAD = (long?)cmd.ExecuteScalar();

            Assert.NotNull(classAD);
            Assert.Equal(4, classAD.Value);

            form.Close();
        }

        // Wrong add: Missing fields and should not be OK.
        [StaFact]
        public void Test7_AddForm_InsertsRecordToDatabase()
        {
            var form = CreateAddForm();

            var nameTextBox = (TextBox)form.Controls.Find("nameTextBox", true).FirstOrDefault();
            var roleComboBox = (ComboBox)form.Controls.Find("roleComboBox", true).FirstOrDefault();
            var houseComboBox = (ComboBox)form.Controls.Find("houseComboBox", true).FirstOrDefault();
            var patronusTextBox = (TextBox)form.Controls.Find("patronusTextBox", true).FirstOrDefault();
            var classUpDown = (NumericUpDown)form.Controls.Find("classUpDown", true).FirstOrDefault();
            var addButton = (Button)form.Controls.Find("addButton", true).FirstOrDefault();
            var cancelButton = (Button)form.Controls.Find("cancelButton", true).FirstOrDefault();

            var assembly = GetCompiledAssembly();
            var formType = assembly.GetType($"{TargetAssembly}.View.Add");

            var fieldInfo = formType.GetField("errorProvider", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var errorProvider = (ErrorProvider?)fieldInfo!.GetValue(form);

            nameTextBox.Text = "False";
            roleComboBox.SelectedItem = "";
            houseComboBox.SelectedItem = "";
            patronusTextBox.Text = " ";
            classUpDown.Value = 0;

            form.Show();
            addButton.PerformClick();

            Assert.True(form.DialogResult == DialogResult.None, "[DB] DialogResult should be None");
            Assert.True(errorProvider!.HasErrors, "[DB] errorProvider should have errors");

            Assert.True(File.Exists(DbPath), "[DB] Database file was not found. Migrations not run or wrong file name.");

            using var connection = new SqliteConnection($"Data Source={DbPath}");
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT Class FROM Wizards WHERE Name = 'False'";
            var classAD = (long?)cmd.ExecuteScalar();

            Assert.Null(classAD);
            form.Close();
        }

        [StaFact]
        public void Test8_ListForm_LoadsDataIntoGrid()
        {
            var assembly = GetCompiledAssembly();
            var contextType = assembly.GetType($"{TargetAssembly}.Context.WizardContext");
            var formType = assembly.GetType($"{TargetAssembly}.View.StartUp");

            Assert.True(formType != null, $"[StartUp] Form '{TargetAssembly}.View.StartUp' was not found.");

            var optionsBuilder = new DbContextOptionsBuilder<WizardContext>();
            optionsBuilder.UseSqlite($"Data Source={DbPath}");

            var context = Activator.CreateInstance(contextType!, optionsBuilder.Options);
            var services = new ServiceCollection();
            services.AddSingleton(contextType!, context!);

            var serviceProvider = services.BuildServiceProvider();
            var form = (Form)Activator.CreateInstance(formType!, serviceProvider)!;

            var gridView = (DataGridView?)form.Controls.Find("wizardGridView", true).FirstOrDefault();
            Assert.True(gridView != null, "[StartUp] Control 'wizardGridView' (DataGridView) was not found on the form.");

            using (var connection = new SqliteConnection($"Data Source={DbPath}"))
            {
                connection.Open();

                using var clearCmd = connection.CreateCommand();
                clearCmd.CommandText = "DELETE FROM Wizards;";
                clearCmd.ExecuteNonQuery();

                using var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO Wizards (Name, Role, Class, House, Patronus) VALUES
                    ('Harry Potter', 'diák', 5, 'Griffendél', 'Stag'),
                    ('Severus Snape', 'oktató', 0, 'Mardekár', 'Doe'),
                    ('Draco Malfoy', 'diák', 5, 'Mardekár', 'None'),
                    ('Hermione Granger', 'diák', 5, 'Griffendél', 'Otter'),
                    ('Minerva McGonagall', 'oktató', 0, 'Griffendél', 'Cat');
                ";
                insertCmd.ExecuteNonQuery();
            }

            form.Show();

            // (Role ASC -> Class ASC -> House ASC)
            var expectedNamesInOrder = new[]
            {
                "Harry Potter",
                "Hermione Granger",
                "Draco Malfoy",
                "Minerva McGonagall",
                "Severus Snape"
            };

            var actualNamesInGrid = new List<string>();
            foreach (DataGridViewRow row in gridView!.Rows)
            {
                if (row.IsNewRow) continue;

                var cellValue = row.Cells["Name"]?.Value?.ToString() ?? row.Cells[0]?.Value?.ToString();

                if (!string.IsNullOrEmpty(cellValue))
                {
                    actualNamesInGrid.Add(cellValue);
                }
            }

            // Quick hack for parallel testing, not to interfere. See: Test6_AddForm_InsertsRecordToDatabase
            actualNamesInGrid.Remove("Ron Weasley");

            Assert.Multiple(
                () => Assert.True(actualNamesInGrid.Count == 5,
                    $"[StartUp] DataGridView should contain 5 rows, but found {actualNamesInGrid.Count}."),

                () =>
                {
                    var studentCountInFirst3 = actualNamesInGrid.Take(3)
                        .Count(name => name == "Harry Potter" || name == "Hermione Granger" || name == "Draco Malfoy");

                    Assert.True(studentCountInFirst3 == 3,
                        "[StartUp] Grid items are not sorted properly by 'Role'. Students should appear before Teachers.");
                },

                () =>
                {
                    int minervaIndex = actualNamesInGrid.IndexOf("Minerva McGonagall");
                    int snapeIndex = actualNamesInGrid.IndexOf("Severus Snape");

                    if (minervaIndex != -1 && snapeIndex != -1)
                    {
                        Assert.True(minervaIndex < snapeIndex,
                            "[StartUp] Grid items are not sorted properly by 'House'. 'Minerva McGonagall' (Gryffindor) should come before 'Severus Snape' (Slytherin).");
                    }
                }
            );

            form.Close();
        }
    }
}
