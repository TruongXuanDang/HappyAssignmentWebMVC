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

            AddStudent("Tran Van An", 7);
            AddStudent("Tran Thi Binh", 7);
            AddStudent("Tran Van Tuan", 7);
            AddStudent("Nguyen Van Can", 7);
            AddStudent("Dang Xuan Trung", 7);
            AddStudent("An Van Long", 7);
            AddStudent("Le Van Tien", 7);
            AddStudent("Tran Xuan Son", 7);
            AddStudent("Nguyen Thi Le", 7);
            AddStudent("Le Van Tam", 7);
            AddStudent("Tran Xuan Long", 7);
            AddStudent("Dang Tan Binh", 7);
            AddStudent("Le Hai Ly", 7);
            AddStudent("Dang Truong An", 7);
            AddStudent("Huynh Van Ba", 7);

            AddStudent("Dang Van Tuan", 8);
            AddStudent("Tran Xuan Binh", 8);
            AddStudent("Truong Van Xuan", 8);
            AddStudent("Truong Gia Binh", 8);
            AddStudent("Ho Ngoc Ha", 8);
            AddStudent("Tran Tuan Anh", 8);
            AddStudent("Tran Xuan Son", 8);
            AddStudent("Dang Le Nguyen Vu", 8);
            AddStudent("Nguyen Thuy Chi", 8);
            AddStudent("Le Van Tuan", 8);
            AddStudent("Dinh Thi Xuan", 8);
            AddStudent("Ho Van Can", 8);
            AddStudent("Tran Thi Xuan", 8);
            AddStudent("Trieu Thi Binh", 8);
            AddStudent("Ngo Tien Dat", 8);

            AddStudent("Leu Van Can", 9);
            AddStudent("Trieu Xuan Son", 9);
            AddStudent("Nguyen Tan Tai", 9);
            AddStudent("Phan Thi Hai Yen", 9);
            AddStudent("Nguyen Thi Vi Bi", 9);
            AddStudent("Truong Gia Binh", 9);
            AddStudent("Truong Van Long", 9);
            AddStudent("Le Thi Ly", 9);
            AddStudent("Phan Van Hoa", 9);
            AddStudent("Phan Van Trung", 9);
            AddStudent("Nguyen Duc Tai", 9);
            AddStudent("Lo Thi Song", 9);
            AddStudent("Tran Van Tuyen", 9);
            AddStudent("Ho Tan Tai", 9);
            AddStudent("Nguyen Xuan Phuc", 9);

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
