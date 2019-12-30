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

            AddStudent("Tran Van An", 4);
            AddStudent("Tran Thi Binh", 4);
            AddStudent("Tran Van Tuan", 4);
            AddStudent("Nguyen Van Can", 4);
            AddStudent("Dang Xuan Trung", 4);
            AddStudent("An Van Long", 4);
            AddStudent("Le Van Tien", 4);
            AddStudent("Tran Xuan Son", 4);
            AddStudent("Nguyen Thi Le", 4);
            AddStudent("Le Van Tam", 4);
            AddStudent("Tran Xuan Long", 4);
            AddStudent("Dang Tan Binh", 4);
            AddStudent("Le Hai Ly", 4);
            AddStudent("Dang Truong An", 4);
            AddStudent("Huynh Van Ba", 4);

            AddStudent("Dang Van Tuan", 5);
            AddStudent("Tran Xuan Binh", 5);
            AddStudent("Truong Van Xuan", 5);
            AddStudent("Truong Gia Binh", 5);
            AddStudent("Ho Ngoc Ha", 5);
            AddStudent("Tran Tuan Anh", 5);
            AddStudent("Tran Xuan Son", 5);
            AddStudent("Dang Le Nguyen Vu", 5);
            AddStudent("Nguyen Thuy Chi", 5);
            AddStudent("Le Van Tuan", 5);
            AddStudent("Dinh Thi Xuan", 5);
            AddStudent("Ho Van Can", 5);
            AddStudent("Tran Thi Xuan", 5);
            AddStudent("Trieu Thi Binh", 5);
            AddStudent("Ngo Tien Dat", 5);

            AddStudent("Leu Van Can", 6);
            AddStudent("Trieu Xuan Son", 6);
            AddStudent("Nguyen Tan Tai", 6);
            AddStudent("Phan Thi Hai Yen", 6);
            AddStudent("Nguyen Thi Vi Bi", 6);
            AddStudent("Truong Gia Binh", 6);
            AddStudent("Truong Van Long", 6);
            AddStudent("Le Thi Ly", 6);
            AddStudent("Phan Van Hoa", 6);
            AddStudent("Phan Van Trung", 6);
            AddStudent("Nguyen Duc Tai", 6);
            AddStudent("Lo Thi Song", 6);
            AddStudent("Tran Van Tuyen", 6);
            AddStudent("Ho Tan Tai", 6);
            AddStudent("Nguyen Xuan Phuc", 6);

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
