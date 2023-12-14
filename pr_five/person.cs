using System;
using System.Collections.Generic;

namespace Лабораторная_1
{
    public enum Gender
    {
        Мужской,
        Женский,
        Неизвестно
    }

    public enum IdType
    {
        Паспорт,
        Свидетельство_о_рождении,
        Неизвестно,
    }

    public class Person
    {
        private string name_;
        private Gender gender_;
        private string id_number_;
        private IdType id_type_;
        private string place_of_birth_;
        private string date_of_birth_;
        private List<Person> Children_ = new List<Person>();
        private Person mother_;
        private Person father_;

        public Person(string name, Gender gender, string idNumber, IdType idType, string placeOfBirth, string dateOfBirth, List<Person> children, Person mother, Person father)
        {
            if (name.Length >= 40)
            {
                throw new ArgumentOutOfRangeException("ФИО не может быть больше 40 символов.");
            }
            foreach (char c in name)
            {
                if ((char.IsDigit(c) || char.IsPunctuation(c)) && c != ' ')
                {
                    throw new ArgumentException("Поле 'ФИО' содержит недопустимые символы.");
                }
            }
            if (idNumber.Length != 11)
            {
                throw new ArgumentOutOfRangeException("Значение поля 'ID' должно быть равно 11 символам.");
            }
            foreach (char c in idNumber)
            {
                if (!char.IsDigit(c) && c != ' ')
                {
                    throw new ArgumentException("Поле 'ID' содержит недопустимые символы.");
                }
            }
            foreach (char c in placeOfBirth)
            {
                if ((char.IsDigit(c) || char.IsPunctuation(c)) && c != ' ' && c != '-')
                {
                    throw new ArgumentException("Поле 'Место рождения' содержит недопустимые символы.");
                }
            }
            if (dateOfBirth.Length != 10)
            {
                throw new ArgumentException("Введен неверный формат даты рождения.");
            }

            string yearString = dateOfBirth.Substring(6, 4);
            if (!int.TryParse(yearString, out int year))
            {
                throw new ArgumentException("Невозможно преобразовать год рождения в число.");
            }
            // Проверяем условие на год
            if (year > 2023)
            {
                throw new ArgumentException("Неправильно введена дата. Год рождения не может быть больше 2023.");
            }

            foreach (char c in dateOfBirth)
            {
                if (!char.IsDigit(c) && c != '.')
                {
                    throw new ArgumentException("Поле 'Дата рождения' содержит недопустимые символы.");
                }
            }

            name_ = name;
            gender_ = gender;
            id_number_ = idNumber;
            id_type_ = idType;
            place_of_birth_ = placeOfBirth;
            date_of_birth_ = dateOfBirth;

            if (idType == IdType.Паспорт && Age < 14)
            {
                throw new ArgumentException("Нельзя использовать ID 'Паспорт' для человека младше 14 лет.");
            }
            if (idType == IdType.Свидетельство_о_рождении && Age > 14)
            {
                throw new ArgumentException("Нельзя использовать Свидетельство о рождении для человека старше 14 лет.");
            }

            if (mother != null)
            {
                if (mother == this)
                {
                    throw new ArgumentException("Человек не может быть своей собственной матерью.");
                }
                if (mother.Gender != Gender.Женский)
                {
                    throw new ArgumentException("Объект 'Мать' имеет неверный пол.");
                }
            }
            mother_ = mother;

            if (father != null)
            {
                if (father_ == this)
                {
                    throw new ArgumentException("Человек не может быть своим собственным отцом.");
                }
                if (father.Gender != Gender.Мужской)
                {
                    throw new ArgumentException("Объект 'Отец' имеет неверный пол.");
                }
            }
            father_ = father;

            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        public string Name
        {
            get { return name_; }
            set { name_ = value; }
        }

        public Gender Gender
        {
            get { return gender_; }
        }

        public string IdNumber
        {
            get { return id_number_; }
            set { id_number_ = value; }
        }

        public IdType IdType
        {
            get { return id_type_; }
            set
            {
                if (value == IdType.Свидетельство_о_рождении && HasPassport)
                {
                    throw new ArgumentException("Нельзя использовать ID 'Свидетельство о рождении' для человека, у которого уже есть паспорт.");
                }
                id_type_ = value;
            }
        }

        public string PlaceOfBirth
        {
            get { return place_of_birth_; }
            set { place_of_birth_ = value; }
        }

        public string DateOfBirth
        {
            get { return date_of_birth_; }
        }

        public int Age
        {
            get
            {
                int day = int.Parse(date_of_birth_.Substring(0, 2));
                int month = int.Parse(date_of_birth_.Substring(3, 2));
                int year = int.Parse(date_of_birth_.Substring(6, 4));
                DateTime today = DateTime.Today;
                int age = today.Year - year;
                if (today.Month < month || (today.Month == month && today.Day < day))
                {
                    age--;
                }
                return age;
            }
        }

        public Person Mother
        {
            get { return mother_; }
            set
            {
                if (value == this)
                {
                    throw new ArgumentException("Человек не может быть своей собственной матерью.");
                }
                if (value.Gender != Gender.Женский)
                {
                    throw new ArgumentException("Объект 'Мать' имеет неверный пол.");
                }
                mother_ = value;
            }
        }

        public Person Father
        {
            get { return father_; }
            set
            {
                if (value == this)
                {
                    throw new ArgumentException("Человек не может быть своим собственным отцом.");
                }
                if (value.Gender != Gender.Мужской)
                {
                    throw new ArgumentException("Объект 'Отец' имеет неверный пол.");
                }
                father_ = value;
            }
        }

        public void AddChild(Person child)
        {
            int maxDepth = 5; //глубина проверки

            if (child == this)
            {
                throw new ArgumentException("Человек не может быть своим собственным ребенком.");
            }

            // Проверка, что ребенок не является ребенком чужих родителей
            if (this.Gender == Gender.Женский && child.Mother != null)
            {
                throw new ArgumentException($"Ребенок {child.name_} уже имеет мать.");
            }

            if (this.Gender == Gender.Мужской && child.Father != null)
            {
                throw new ArgumentException($"Ребенок {child.name_} уже имеет отца.");
            }

            if (!child.IsAncestor(this, maxDepth))
            {
                Children_.Add(child);
            }
            else
            {
                throw new ArgumentException($"Ошибка: Обнаружен цикл в родственных связях, {name_} не может быть родителем человека: {child.name_}");
            }

            // Проверка возраста ребенка
            if (child.Age >= this.Age)
            {
                throw new ArgumentException($"Ребенок должен быть младше родителя.");
            }

            // Добавление ребенка к родителю
            if (this.Gender == Gender.Женский)
            {
                child.Mother = this;
            }
            else
            {
                child.Father = this;
            }
        }

        public bool IsAncestor(Person potentialChild, int maxDepth)
        {
            if (maxDepth <= 0)
            {
                return false;
            }
            foreach (var child in Children_)
            {
                if (child == potentialChild || child.IsAncestor(potentialChild, maxDepth - 1))
                {
                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<Person> Children
        {
            get { return Children_.AsReadOnly(); }
        }

        public bool HasPassport
        {
            get { return id_type_ == IdType.Паспорт; }
        }

    }
}

