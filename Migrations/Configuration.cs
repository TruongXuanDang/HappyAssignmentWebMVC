namespace HappyMVCAssignment.Migrations
{
    using HappyMVCAssignment.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using static HappyMVCAssignment.Models.Classroom;
    using static HappyMVCAssignment.Models.Student;

    internal sealed class Configuration : DbMigrationsConfiguration<HappyMVCAssignment.Models.HappyMVCAssignmentContext>
    {
        List<Classroom> listClassroom = new List<Classroom>();
        List<Student> listStudent = new List<Student>();
        List<LateEvent> listLateEvent = new List<LateEvent>();
        List<LateSetting> listLateSetting = new List<LateSetting>();
        Random gen = new Random();
        double moneyPerLate = 20000;
        int pushPerLate = 10;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "HappyMVCAssignment.Models.HappyMVCAssignmentContext";
            
        }

        protected override void Seed(HappyMVCAssignment.Models.HappyMVCAssignmentContext context)
        {
            AddClassroom("T1806E");
            AddClassroom("T1807E");
            AddClassroom("T1808E");

            AddStudent("Tran Van An", 1);
            AddStudent("Tran Thi Binh", 1);
            AddStudent("Tran Van Tuan", 1);
            AddStudent("Nguyen Van Can", 1);
            AddStudent("Dang Xuan Trung", 1);
            AddStudent("An Van Long", 1);
            AddStudent("Le Van Tien", 1);
            AddStudent("Tran Xuan Son", 1);
            AddStudent("Nguyen Thi Le", 1);
            AddStudent("Le Van Tam", 1);
            AddStudent("Tran Xuan Long", 1);
            AddStudent("Dang Tan Binh", 1);
            AddStudent("Le Hai Ly", 1);
            AddStudent("Dang Truong An", 1);
            AddStudent("Huynh Van Ba", 1);

            AddStudent("Dang Van Tuan", 2);
            AddStudent("Tran Xuan Binh", 2);
            AddStudent("Truong Van Xuan", 2);
            AddStudent("Truong Gia Binh", 2);
            AddStudent("Ho Ngoc Ha", 2);
            AddStudent("Tran Tuan Anh", 2);
            AddStudent("Tran Xuan Son", 2);
            AddStudent("Dang Le Nguyen Vu", 2);
            AddStudent("Nguyen Thuy Chi", 2);
            AddStudent("Le Van Tuan", 2);
            AddStudent("Dinh Thi Xuan", 2);
            AddStudent("Ho Van Can", 2);
            AddStudent("Tran Thi Xuan", 2);
            AddStudent("Trieu Thi Binh", 2);
            AddStudent("Ngo Tien Dat", 2);

            AddStudent("Leu Van Can", 3);
            AddStudent("Trieu Xuan Son", 3);
            AddStudent("Nguyen Tan Tai", 3);
            AddStudent("Phan Thi Hai Yen", 3);
            AddStudent("Nguyen Thi Vi Bi", 3);
            AddStudent("Truong Gia Binh", 3);
            AddStudent("Truong Van Long", 3);
            AddStudent("Le Thi Ly", 3);
            AddStudent("Phan Van Hoa", 3);
            AddStudent("Phan Van Trung", 3);
            AddStudent("Nguyen Duc Tai", 3);
            AddStudent("Lo Thi Song", 3);
            AddStudent("Tran Van Tuyen", 3);
            AddStudent("Ho Tan Tai", 3);
            AddStudent("Nguyen Xuan Phuc", 3);

            for (var i = 0; i < 100; i++)
            {
                AddLateEvent();
            }

            AddLateSetting();

            context.Classrooms.AddRange(listClassroom);
            context.Students.AddRange(listStudent);
            context.LateEvents.AddRange(listLateEvent);
            context.LateSettings.AddRange(listLateSetting);
            context.SaveChanges();
        }

        private void AddClassroom(string name, ClassStatus classStatus = ClassStatus.Active)
        {
            listClassroom.Add(new Classroom
            {
                Name = name,
                Status = classStatus
            });
        }

        private void AddStudent(string name,int classroomId, StudentStatus status = StudentStatus.Active)
        {
            var classroom = listClassroom.Find(m => m.Id == classroomId);
            var prefixCode = "Student";
            int id = gen.Next(0, 9999);

            var prefixPhone = "097877";

            listStudent.Add(new Student
            {
                Name = name,
                Code = prefixCode + id.ToString("0000"),
                Phone = prefixPhone + id.ToString("0000"),
                Email = name.ToLower().Trim()+"@gmail.com",
                Status = status,
                ClassroomId = classroomId,
                Classroom = classroom
            });
        }

        private void AddLateEvent()
        {
            DateTime start = new DateTime(2017, 1, 1);
            int range = (DateTime.Today - start).Days;

            Array values = Enum.GetValues(typeof(LateEvent.Type));
            LateEvent.Type lateType = (LateEvent.Type)values.GetValue(gen.Next(values.Length));

            double lateMoney = 0;
            int pushCount = 0;

            int studentId = gen.Next(1,45);

            if (lateType == LateEvent.Type.Money)
            {
                lateMoney = moneyPerLate;
            }
            else if(lateType == LateEvent.Type.Push)
            {
                pushCount = pushPerLate;
            }

            listLateEvent.Add(new LateEvent
            {
                LateDate = start.AddDays(gen.Next(range)),
                LateType = lateType,
                LateMoney = lateMoney,
                PushCount = pushCount,
                StudentId = studentId,
                Student = listStudent.Find(m=>m.Id == studentId)
            });
        }

        private void AddLateSetting()
        {
            listLateSetting.Add(new LateSetting { 
                MoneyPerLate = moneyPerLate,
                PushPerLate = pushPerLate,
                SecondRate = 0,
                ThirdRate =0,
            });
        }
    }
}
