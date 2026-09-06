using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System.Windows.Forms;

namespace ESU
{
    class Generator
    {
        string[] department_name = new string[]
        {
            "Солидарность", "Оберег", "Сохраннось", "Малибу", "Активисты", "Волонтеры", "Безопасность", "Неприкосновенность", "Результативность", "Биосфера", "Потомки",
            "Экологизм", "Природознавство", "Велес", "Сварог", "Компромисс", "Благодарность", "Осторожность", "Береженный", "Люди в зеленом", "Сорока", "Зеленые", "Торчвик",
            "Надзорные", "Зеленый гай", "Мир", "Благополучие", "Предусмотрительность", "Предупреждение", "Резервация"
        };

        string[] company_name = new string[]
        {
            "Авангард", "Промдом", "В лучший мир", "Теремок", "ОПТ", "Пахающие пчелки", "Гордость", "Трудяги", "Йокохама", "Арселор", "Синергетик", "Ударное", "Державный",
            "Народный", "Проспект", "Результат", "Респект", "Высокий", "Димитровский", "Дьяк", "Барин", "Очумелые ручки", "Генералка", "Мастеровые", "Дали", "Липсар", "Метро",
            "Маэстро", "Геркулес", "Червонец", "Мощь", "Работка", "Обработяги", "Бархатные трудяги", "Закалка", "Домик в деревне", "Фигурин", "Прайонис", "Император", "Чумак", 
            "Сошипешник", "Ёмай-ё", "Баланс", "Золотая эра", "Благодон", "Узьмаковск", "Лебрихцы", "Работа народа", "Первак", "Перекати-поле", "Казачек", "Хмельницк", "Дружба"
        };

        string[] prefix_phone = new string[] 
        { "+7", "+38" };

        string[] FAMILIAS_M = new string[]
        {
            "Айдин","Иванов","Демьянов","Грефов", "Сугорицкий", "Остервайс", "Чурубасов", "Сендропол", "Хершер", "Кусто", "Амброзиус", "Кицураги", "Шипшенишный", "Борецкий", "Егоров", "Золотоносный", "Туканов","Черных",
            "Белых","Серых","Младой","Казачевский","Стопарник","Властитель","Семицкий","Даромир","Борисов","Бояр","Ходильник","Жиров","Паулюс","Вольный","Думан","Умский","Ельениц","Киевский","Краш","Елутник"
        };
        string[] FAMILIAS_F = new string[]
       {
            "Айдина","Иванова","Демьянова","Грефовна", "Сугорицкая", "Остервайс", "Чурубасова", "Сендропол", "Хершер", "Кусто", "Амброзиус", "Кицураги", "Шипшенишная", "Борецккая", "Егорова", "Золотоносная", "Туканова",
            "Черных","Белых","Серых","Младая","Казачевская","Стопарник","Властитель","Семицкая","Даромир","Борисова","Боярова","Ходильник","Жирова","Паулюс","Вольная","Думанова","Умская","Ельениц","Киевская","Краша","Елутная"
       };

        string[] NAMES_M = new string[]
        {
            "Сергей", "Хасан", "Раис", "Джулиан", "Жуан", "Зюзя", "Аркентий", "Аки", "Хартман", "Дожа", "Сергар", "Сергий", "Захар", "Егор", "Павел", "Михаил", "Аркадий", "Давид",
            "Кирилл", "Роман", "Тимофей", "Степан", "Владимир", "Фёдор", "Игорь", "Воланд", "Чукча", "Людовик", "Генрих", "Инджрих", "Иллидан", "Вельт", "Мигель", "Рафаэль", "Родион", 
            "Ким"
        };
        string[] NAMES_F = new string[]
        {
            "Зинаида", "Лариса", "Валерия", "Джулиана", "Жуана", "Милана", "Василиса", "Аркадия", "Ксения", "Екатерина", "Кертерсия", "Ольга", "Есения", "Любовь", "Арина"
            , "Вероника", "Ева", "Мишель", "Изольда", "Кризель", "Лоренция", "Лаванда", "Юлия", "Тамара", "Евгения", "Азура", "Марта", "Маргарет", "Мария", "Афина", "Клара", "Алёна", "Нюша", "Настя", "Пайп" 
        };

        string[] THIRD_NAMES_M = new string[]
        {
            "Анатольевич", "Константинович", "Иванович", "Александрович", "Сергеевич", "Старший", "Младший", "Средний", "Евгениевич", "Аркадиевич", "Михайлович", "Терентиевич", "Федорович", "Валерьевич", "Чукчевич",
            "Тарасович","Леонидович","Степанович","Владимирович","Игоревич","Зюзевич","Романович","Родионович","Давидович","Егорович","Игоревич","Захарович","Маратович","Антонович","Олегович","Добрыневич","","","",""
        };
        string[] THIRD_NAMES_F = new string[]
        {
            "Анатольевна", "Константиновна", "Ивановна", "Александровна", "Сергеевна", "Старшая", "Младшая", "Средняя", "Евгениевна", "Аркадиевна", "Михайловна", "Терентиевна", "Федоровна", "Валерьевна", "Чукчевична",
            "Тарасовна","Леонидовна","Степановна","Владимировна","Игоревна","Зюзевна","Романовна","Родионовна","Давидовна","Егоровна","Игоревна","Захаровна","Маратовна","Антоновна","Олеговна","Добрыневна","","","",""
        };

        NpgsqlConnection conn;
        NpgsqlCommand comm;
        
        public Generator(NpgsqlConnection c) 
        {
            conn = c;
        }
        public Generator(NpgsqlConnection c, bool jsonishe)
        {
            conn = c;
            if (jsonishe)
                RunJSON();
            else
                RunPartitions();
        }

        public void RunPartitions() 
        {
            Random rnd = new Random();
            string disaster_date;
            for (int i = 0; i < 50000; i++)
            {
                disaster_date = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1600, DateTime.UtcNow.Date.Year) + "\')::date";
                comm = new NpgsqlCommand("INSERT INTO public.partition1(disaster_date, id_company, id_department, id_type_disaster, casualtys) " +
                    "VALUES(" + disaster_date + ", " + rnd.Next(1, 500) + ", " + rnd.Next(1, 100) + ", " + rnd.Next(1, 10) + ", " + rnd.Next(1, 10000) + ");", conn);
                comm.ExecuteNonQuery();
            }

        }

        public void RunJSON() 
        {
            Random rnd = new Random();
            string disaster_date, type_disaster, name_company, city_company, industry, opening_company, phone_company, name_department,
                city_department, opening_department, phone_department, first_name, second_name, third_name, birth_date, 
                phone, count_people, count_days;
            DataTable dt_cities = new DataTable();
            DataTable dt_industries = new DataTable();
            DataTable dt_type_disaster = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM cities", conn);
            dt_cities.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM type_disaster", conn);
            dt_type_disaster.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM industries", conn);
            dt_industries.Load(comm.ExecuteReader());
            int workers;
            workers = 0;

            for (int i =0; i<10000; i++)
            {
                
                city_company = dt_cities.Rows[rnd.Next(dt_cities.Rows.Count)][1].ToString();
                disaster_date = rnd.Next(1900, DateTime.UtcNow.Date.Year) + "-" + rnd.Next(1, 13).ToString() + "-" + rnd.Next(1, 29).ToString();
                industry = dt_industries.Rows[rnd.Next(dt_industries.Rows.Count)][1].ToString();
                type_disaster = dt_type_disaster.Rows[rnd.Next(dt_type_disaster.Rows.Count)][1].ToString();
                name_company = company_name[rnd.Next(company_name.Length)];
                opening_company = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                phone_company = prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                   + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString();

                string command = "INSERT INTO public.engagement_json(data) VALUES(\'{ \"engagements\": [{ \"disaster\": { \"disaster_date\": " +
                    "\""+disaster_date+"\", \"type_disaster\": \""+type_disaster+"\", \"company\": { \"name_company\": \""+name_company+"\", \"city_company\": \""+city_company+"\", " +
                    "\"industry\": \""+industry+"\", \"opening_company\": "+opening_company+", \"phone_company\": \""+ phone_company + "\" } }, \"departments\": " +
                    "[";

                int departments = rnd.Next(1, 6);
                for (int d = 0; d < departments; d++)
                {
                    name_department = department_name[rnd.Next(department_name.Length)];
                    city_department = dt_cities.Rows[rnd.Next(dt_cities.Rows.Count)][1].ToString();
                    opening_department = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                    phone_department = prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                   + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString();
                    command += "{ \"name_department\": \""+name_department+"\", \"city_department\": \""+city_department+"\", \"opening_department\": "+opening_department+", " +
                        "\"phone_department\": \""+phone_department+"\", \"employees\": [";
                    workers = rnd.Next(1, 20);
                    for (int j = 0; j < workers; j++)
                    {
                        first_name = ""; second_name = ""; third_name = "";
                        switch (rnd.Next(2))
                        {
                            case 0:
                                first_name =  FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)];
                                second_name = NAMES_M[rnd.Next(NAMES_M.Length)];
                                third_name = THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)];
                                break;
                            case 1:
                                first_name = FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)];
                                second_name = NAMES_F[rnd.Next(NAMES_F.Length)];
                                third_name =  THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)];
                                break;
                        }
                        birth_date = rnd.Next(1900, DateTime.UtcNow.Date.Year-18) + "-" + rnd.Next(1, 13).ToString() + "-" + rnd.Next(1, 29).ToString(); ;
                        phone = prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString();
                        command += "{ \"first_name\": \""+first_name+"\", \"second_name\": \""+second_name+"\", \"third_name\": \""+third_name+"\", " +
                            "\"birth_date\": \""+ birth_date + "\", \"phone\": \""+phone+"\" }";
                        if (j == workers - 1)
                            command += "] ";
                        else
                            command += ", ";
                    }
                    if (d == departments - 1)
                        command+="} ], ";
                    else
                        command+="}, ";
                }
                count_people = "";
                count_days = "";
                command += "\"count_people\": "+rnd.Next(1,workers)+", \"count_days\": "+rnd.Next(1,500)+" }] }\'::jsonb); ";
                comm = new NpgsqlCommand(command, conn);
                comm.ExecuteNonQuery();

            }
            MessageBox.Show("Готово");
        }

        public void Run()
        {
            ClearTables();
            GenerateDepartments();
            GenerateWorkers();
            GenerateCompanies();
            GenerateDisasters();
            GenerateEngagements();
            GenerateConsequences();
            MessageBox.Show("Готово");
        }
        public void ClearTables()
        {
            DataTable dt_delete_workers = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM workers_all_view WHERE \"Логин\" <> \'postgres\'", conn);
            dt_delete_workers.Load(comm.ExecuteReader());
            foreach(DataRow row in dt_delete_workers.Rows)
            {
                comm = new NpgsqlCommand("SELECT * FROM delete_worker(" + row[0].ToString() + ")", conn);
                comm.ExecuteNonQuery();
            }
            comm = new NpgsqlCommand("TRUNCATE consequences, engagement, companies, disasters, workers, departments, workers_old RESTART IDENTITY;", conn);
            comm.ExecuteNonQuery();
        }
        public void GenerateDepartments()
        {
            DataTable dt_cities = new DataTable(); 
            comm = new NpgsqlCommand("SELECT * FROM cities", conn);
            dt_cities.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string name, id_city, phone_number, year, employees;
            foreach (DataRow row in dt_cities.Rows)
            {
                name = "\'" + department_name[rnd.Next(department_name.Length)] + "\'";
                id_city = row[0].ToString();
                phone_number = "\'"+prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString()+ "\'";
                year = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                employees = rnd.Next(0, 30).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.departments(department_name, id_city, opening_year, employees, phone) VALUES"
                    + "("+name+", "+id_city+", "+year+", "+employees+", "+phone_number+")", conn);
                comm.ExecuteNonQuery();
            }
            for (int i = 0; i < 204; i++)
            {
                name = "\'" + department_name[rnd.Next(department_name.Length)] + "\'";
                id_city = dt_cities.Rows[rnd.Next(dt_cities.Rows.Count)][0].ToString();
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                year = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                employees = rnd.Next(0, 30).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.departments(department_name, id_city, opening_year, employees, phone) VALUES"
                    + "(" + name + ", " + id_city + ", " + year + ", " + employees + ", " + phone_number + ")", conn);
                comm.ExecuteNonQuery();
            }
        }
        public void GenerateWorkers() 
        {
            comm = new NpgsqlCommand("INSERT INTO public.workers(id_department, first_name, second_name, third_name, date_birth, \"position\", login, phone_number) VALUES " +
                "(1, \'Айдин\', \'Сергей\', \'Анатольевич\', \'25.03.2003\', \'Администратор\', \'postgres\', \'+7 949 429 42-51\');", conn);
            comm.ExecuteNonQuery();
            DataTable dt_departments = new DataTable();
            comm = new NpgsqlCommand("SELECT id_department FROM departments", conn);
            dt_departments.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string id_department,first_name="",second_name = "", third_name = "", date_birth,position,login,phone_number;
            int seq = 0;
            foreach (DataRow row in dt_departments.Rows)
            {
                id_department = row[0].ToString();
                switch (rnd.Next(2)) 
                {
                    case 0:
                        first_name = "\'" + FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)] + "\'";
                        second_name = "\'" + NAMES_M[rnd.Next(NAMES_M.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)] + "\'";
                        break;
                    case 1:
                        first_name = "\'" + FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)] + "\'";
                        second_name = "\'" + NAMES_F[rnd.Next(NAMES_F.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)] + "\'";
                        break;
                }
                date_birth = "(\'" + rnd.Next(1, 29).ToString() + "." +rnd.Next(1,13).ToString()+ "." +rnd.Next(1900, DateTime.UtcNow.Date.Year-18)+ "\')::date";
                position = "\'" + "Глава отдела" + "\'";
                login = "\'" + "hd_" + id_department + "_1" + "\'";
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                comm = new NpgsqlCommand("SELECT * FROM add_worker("+id_department+", "+first_name+", "+second_name+", "+third_name
                    +", "+date_birth+", "+position+", "+login+", "+phone_number+", \'1234\');", conn);
                comm.ExecuteNonQuery();
            }
            foreach (DataRow row in dt_departments.Rows)
            {
                id_department = row[0].ToString();
                switch (rnd.Next(2))
                {
                    case 0:
                        first_name = "\'" + FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)] + "\'";
                        second_name = "\'" + NAMES_M[rnd.Next(NAMES_M.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)] + "\'";
                        break;
                    case 1:
                        first_name = "\'" + FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)] + "\'";
                        second_name = "\'" + NAMES_F[rnd.Next(NAMES_F.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)] + "\'";
                        break;
                }
                date_birth = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1900, DateTime.UtcNow.Date.Year - 18) + "\')::date";
                position = "\'" + "Сотрудник" + "\'";
                login = "\'" + "ed_" + id_department + "_1" + "\'";
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                comm = new NpgsqlCommand("SELECT * FROM add_worker(" + id_department + ", " + first_name + ", " + second_name + ", " + third_name
                    + ", " + date_birth + ", " + position + ", " + login + ", " + phone_number + ", \'1234\');", conn);
                comm.ExecuteNonQuery();
            }
            for (int i = 0; i < 1700; i++)
            {
                id_department = dt_departments.Rows[rnd.Next(dt_departments.Rows.Count)][0].ToString();
                switch (rnd.Next(2))
                {
                    case 0:
                        first_name = "\'" + FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)] + "\'";
                        second_name = "\'" + NAMES_M[rnd.Next(NAMES_M.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)] + "\'";
                        break;
                    case 1:
                        first_name = "\'" + FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)] + "\'";
                        second_name = "\'" + NAMES_F[rnd.Next(NAMES_F.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)] + "\'";
                        break;
                }
                date_birth = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1900, DateTime.UtcNow.Date.Year - 18) + "\')::date";
                position = "\'" + "Сотрудник" + "\'";
                login = "\'" + "ed_" + id_department + "_"+(i+2).ToString()+ "\'";
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                comm = new NpgsqlCommand("SELECT * FROM add_worker(" + id_department + ", " + first_name + ", " + second_name + ", " + third_name
                    + ", " + date_birth + ", " + position + ", " + login + ", " + phone_number + ", \'1234\');", conn);
                comm.ExecuteNonQuery();
            }
            for (int i = 0; i < 5; i++)
            {
                id_department = dt_departments.Rows[rnd.Next(dt_departments.Rows.Count)][0].ToString();
                switch (rnd.Next(2))
                {
                    case 0:
                        first_name = "\'" + FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)] + "\'";
                        second_name = "\'" + NAMES_M[rnd.Next(NAMES_M.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)] + "\'";
                        break;
                    case 1:
                        first_name = "\'" + FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)] + "\'";
                        second_name = "\'" + NAMES_F[rnd.Next(NAMES_F.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)] + "\'";
                        break;
                }
                date_birth = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1900, DateTime.UtcNow.Date.Year - 18) + "\')::date";
                position = "\'" + "Модератор" + "\'";
                login = "\'" + "mod_" + (i+1).ToString() + "\'";
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                comm = new NpgsqlCommand("SELECT * FROM add_worker(" + id_department + ", " + first_name + ", " + second_name + ", " + third_name
                    + ", " + date_birth + ", " + position + ", " + login + ", " + phone_number + ", \'1234\');", conn);
                comm.ExecuteNonQuery();
            }
            for (int i = 0; i < 20; i++)
            {
                id_department = dt_departments.Rows[rnd.Next(dt_departments.Rows.Count)][0].ToString();
                switch (rnd.Next(2))
                {
                    case 0:
                        first_name = "\'" + FAMILIAS_M[rnd.Next(FAMILIAS_M.Length)] + "\'";
                        second_name = "\'" + NAMES_M[rnd.Next(NAMES_M.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_M[rnd.Next(THIRD_NAMES_M.Length)] + "\'";
                        break;
                    case 1:
                        first_name = "\'" + FAMILIAS_F[rnd.Next(FAMILIAS_F.Length)] + "\'";
                        second_name = "\'" + NAMES_F[rnd.Next(NAMES_F.Length)] + "\'";
                        third_name = "\'" + THIRD_NAMES_F[rnd.Next(THIRD_NAMES_F.Length)] + "\'";
                        break;
                }
                date_birth = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1900, DateTime.UtcNow.Date.Year - 18) + "\')::date";
                position = "\'" + "Менеджер" + "\'";
                login = "\'" + "man_" + (i + 1).ToString() + "\'";
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                comm = new NpgsqlCommand("SELECT * FROM add_worker(" + id_department + ", " + first_name + ", " + second_name + ", " + third_name
                    + ", " + date_birth + ", " + position + ", " + login + ", " + phone_number + ", \'1234\');", conn);
                comm.ExecuteNonQuery();
            }
        }
        public void GenerateCompanies()
        {
            DataTable dt_cities = new DataTable();
            DataTable dt_industries = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM cities", conn);
            dt_cities.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM industries", conn);
            dt_industries.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string name, id_city, id_industry, opening_year, phone_number;
            foreach (DataRow row in dt_cities.Rows)
            {
                name = "\'" + company_name[rnd.Next(company_name.Length)] + "\'";
                id_city = row[0].ToString();
                id_industry = dt_industries.Rows[rnd.Next(dt_industries.Rows.Count)][0].ToString();
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                opening_year = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.companies(name, id_city, id_industry, opening_year, phone) " +
                    "VALUES("+name+ ", " + id_city + ", " + id_industry + ", " + opening_year + ", " + phone_number + ");", conn);
                comm.ExecuteNonQuery();
            }
            for (int i = 0; i < 904; i++)
            {
                name = "\'" + company_name[rnd.Next(company_name.Length)] + "\'";
                id_city = dt_cities.Rows[rnd.Next(dt_cities.Rows.Count)][0].ToString();
                id_industry = dt_industries.Rows[rnd.Next(dt_industries.Rows.Count)][0].ToString();
                phone_number = "\'" + prefix_phone[rnd.Next(2)] + " " + rnd.Next(100, 1000).ToString() + " " + rnd.Next(100, 1000).ToString()
                    + " " + rnd.Next(10, 100).ToString() + "-" + rnd.Next(10, 100).ToString() + "\'";
                opening_year = rnd.Next(1800, DateTime.UtcNow.Date.Year).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.companies(name, id_city, id_industry, opening_year, phone) " +
                    "VALUES(" + name + ", " + id_city + ", " + id_industry + ", " + opening_year + ", " + phone_number + ");", conn);
                comm.ExecuteNonQuery();
            }
        }
        public void GenerateDisasters()
        {
            DataTable dt_companies = new DataTable();
            DataTable dt_type_disaster = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM companies", conn);
            dt_companies.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM type_disaster", conn);
            dt_type_disaster.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string id_company, id_type_disaster, disaster_date;
            for (int i = 0; i < 2500; i++)
            {
                id_company = dt_companies.Rows[rnd.Next(dt_companies.Rows.Count)][0].ToString();
                id_type_disaster = dt_type_disaster.Rows[rnd.Next(dt_type_disaster.Rows.Count)][0].ToString();
                disaster_date = "(\'" + rnd.Next(1, 29).ToString() + "." + rnd.Next(1, 13).ToString() + "." + rnd.Next(1900, DateTime.UtcNow.Date.Year) + "\')::date";
                comm = new NpgsqlCommand("INSERT INTO public.disasters(id_company, id_type_disaster, disaster_date) " +
                    "VALUES ("+id_company+", "+id_type_disaster+", "+disaster_date+");", conn);
                comm.ExecuteNonQuery();
            }
        }
        public void GenerateEngagements()
        {
            DataTable dt_disasters = new DataTable();
            DataTable dt_departments = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM disasters", conn);
            dt_disasters.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM departments", conn);
            dt_departments.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string id_disaster, id_department, count_people, count_days;
            int idDep;
            for (int i = 0; i < 2000; i++)
            {
                id_disaster = dt_disasters.Rows[rnd.Next(dt_disasters.Rows.Count)][0].ToString();
                idDep = rnd.Next(dt_departments.Rows.Count);
                id_department = dt_departments.Rows[idDep][0].ToString();
                count_people = rnd.Next(1, Int32.Parse(dt_departments.Rows[idDep][4].ToString()) + 1).ToString();
                count_days = rnd.Next(1, 300).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.engagement(id_disaster, id_department, count_people, count_days) " +
                    "VALUES ("+id_disaster+", "+id_department+", "+count_people+", "+count_days+");", conn);
                comm.ExecuteNonQuery();
            }
        }
        public void GenerateConsequences()
        {
            DataTable dt_disasters = new DataTable();
            DataTable dt_kind_disaster = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM disasters", conn);
            dt_disasters.Load(comm.ExecuteReader());
            comm = new NpgsqlCommand("SELECT * FROM kind_disaster", conn);
            dt_kind_disaster.Load(comm.ExecuteReader());
            Random rnd = new Random();
            string id_disaster, id_kind_disaster, damage_property, casualtys;
            int idDep;
            for (int i = 0; i < 2000; i++)
            {
                id_disaster = dt_disasters.Rows[rnd.Next(dt_disasters.Rows.Count)][0].ToString();
                id_kind_disaster = dt_kind_disaster.Rows[rnd.Next(dt_kind_disaster.Rows.Count)][0].ToString();
                damage_property = (rnd.Next(1000000) + rnd.Next(10)*0.1).ToString().Replace(',','.');
                casualtys = rnd.Next(0, 50000).ToString();
                comm = new NpgsqlCommand("INSERT INTO public.consequences(id_disaster, id_kind_disaster, damage_property, casualtys) " +
                    "VALUES ("+id_disaster+", "+id_kind_disaster+", "+damage_property+", "+casualtys+");", conn);
                comm.ExecuteNonQuery();
            }
        }
    }
}
