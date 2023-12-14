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
    public partial class Form1 : Form
    {
        
        public void UpdateDataGridView(Person newPerson)
        {
            // Проверяем, есть ли информация о матери
            string motherName = (newPerson.Mother != null) ? newPerson.Mother.Name : "Нет информации о матери";
            string fatherName = (newPerson.Father != null) ? newPerson.Father.Name : "Нет информации об отце";
            dataGridView1.Rows.Add(newPerson.Name, newPerson.Gender, newPerson.IdNumber, newPerson.IdType, newPerson.PlaceOfBirth, newPerson.DateOfBirth, motherName, fatherName); //колонки который обновляют таблицу
        }

        private List<Person> peopleList; // Создание списка для хранения экземпляров класса Person

        public Form1()
        {
            InitializeComponent();
            peopleList = new List<Person>(); // Инициализация списка в конструкторе формы
        }
        public void AddPerson(Person person) //добавить человека в список
        {
            peopleList.Add(person);
        }



        private void button1_Click(object sender, EventArgs e) //кнопка добавить человека
        {
            Person selectedPerson = null; //потенциальный родитель
            try
            {
                using (Form2 f2 = new Form2(selectedPerson, this)) 
                {
                    f2.ShowDialog(this);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, выбран ли родитель (человек)
                if (dataGridView1.SelectedRows.Count > 0 && dataGridView1.SelectedRows[0].Index != dataGridView1.RowCount - 1) // Проверка, что хотя бы один родитель выбран в DataGridView
                {
                    // Получаем индекс выбранной строки
                    int rowIndex = dataGridView1.SelectedRows[0].Index;

                    // Получаем объект Person из выбранной строки DataGridView
                    Person selectedPerson = peopleList[rowIndex];

                    // Открываем Form2 для добавления ребенка выбранному родителю
                    using (Form2 f2 = new Form2(selectedPerson, this)) //добавляем выбранного человека к новому
                    {
                        f2.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show("Выделите всю строку родителя, чтобы добавить ребенка.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.SelectedRows.Count == 2 && dataGridView1.SelectedRows[0].Index != dataGridView1.RowCount - 1)
                {
                    int firstSelectedRowIndex = dataGridView1.SelectedRows[0].Index;
                    int secondSelectedRowIndex = dataGridView1.SelectedRows[1].Index;

                    Person p1 = peopleList[firstSelectedRowIndex];
                    Person p2 = peopleList[secondSelectedRowIndex];

                    if (p1.Age > p2.Age) // 1 - родитель второго
                    {
                        p1.AddChild(p2); // Добавляем второго человека в список детей первого
                        if (p1.Gender == Gender.Женский)
                        {
                            p2.Mother = p1;
                            dataGridView1.Rows[secondSelectedRowIndex].Cells[6].Value = p2.Mother.Name;
                        }

                        else
                        {
                            p2.Father = p1;
                            dataGridView1.Rows[secondSelectedRowIndex].Cells[7].Value = p2.Father.Name;
                        }
                    }
                    else
                    {
                        p2.AddChild(p1); // Добавляем первого человека в список детей второго
                        if (p2.Gender == Gender.Женский)
                        {
                            p1.Mother = p2;
                            dataGridView1.Rows[firstSelectedRowIndex].Cells[6].Value = p1.Mother.Name;
                        }
                        else
                        {
                            p1.Father = p2;
                            dataGridView1.Rows[firstSelectedRowIndex].Cells[7].Value = p1.Father.Name;
                        }
                    }
                    dataGridView1.Refresh();
                }
                else
                {
                    MessageBox.Show("Необходимо выделить двух людей для продолжения операции.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"\nОшибка: {exception.Message} \n");
            }
        }

    }
}
