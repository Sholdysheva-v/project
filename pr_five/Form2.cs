using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Лабораторная_1;

namespace pr_five
{
    public partial class Form2 : Form
    {
        private Form1 mainForm; 
        public Person selectedPerson; 
        public Form2(Person person, Form1 form1) 
        {
            InitializeComponent();
            selectedPerson = person;
            mainForm = form1; 
        }

        private void Добавление_человека_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) //ФИО
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //Пол
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e) //Номер документа
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)//Тип документа
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)//Место рождения
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)//Дата рождения
        {

        }

        private void button1_Click(object sender, EventArgs e) // Кнопка "Добавить"
        {
            try
            {
                // Считываем данные из полей формы Form2
                string name = textBox1.Text;
                Enum.TryParse(comboBox1.SelectedItem?.ToString(), out Gender gender); // Пол
                string idNumber = textBox3.Text;
                Enum.TryParse(comboBox2.SelectedItem?.ToString(), out IdType idType); // Тип документа
                string placeOfBirth = textBox4.Text;
                string dateOfBirth = textBox2.Text;
                List<Person> Children_ = new List<Person>(); 
                Person mother = null;
                Person father = null;

                if (selectedPerson != null)
                {
                    if (selectedPerson.Gender == Gender.Женский)
                    {
                        mother = selectedPerson; 
                    }
                    else
                    {
                        father = selectedPerson; 
                    }
                }

                // Создаем экземпляр класса Person с введенными данными и родителями
                Person newPerson1 = new Person(name, gender, idNumber, idType, placeOfBirth, dateOfBirth, new List<Person>(), mother, father);

                mainForm.UpdateDataGridView(newPerson1);
                mainForm.AddPerson(newPerson1);
                

                // Закрываем форму Form2
                this.Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void button2_Click(object sender, EventArgs e) // Кнопка "Отменить"
        {
            Close(); // Закрытие формы Form2 при отмене
        }
    }
}
