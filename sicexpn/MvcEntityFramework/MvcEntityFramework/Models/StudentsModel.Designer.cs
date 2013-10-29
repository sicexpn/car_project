﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
#region EDM 关系源元数据

[assembly: EdmRelationshipAttribute("AdminModel", "FK_Reports_questions", "question", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(MvcEntityFramework.Models.question), "Reports", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(MvcEntityFramework.Models.report), true)]
[assembly: EdmRelationshipAttribute("AdminModel", "FK_Reports_Students", "Students", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(MvcEntityFramework.Models.student), "report", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(MvcEntityFramework.Models.report), true)]

#endregion

namespace MvcEntityFramework.Models
{
    #region 上下文
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    public partial class AdminEntities : ObjectContext
    {
        #region 构造函数
    
        /// <summary>
        /// 请使用应用程序配置文件的“AdminEntities”部分中的连接字符串初始化新 AdminEntities 对象。
        /// </summary>
        public AdminEntities() : base("name=AdminEntities", "AdminEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 AdminEntities 对象。
        /// </summary>
        public AdminEntities(string connectionString) : base(connectionString, "AdminEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// 初始化新的 AdminEntities 对象。
        /// </summary>
        public AdminEntities(EntityConnection connection) : base(connection, "AdminEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region 分部方法
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet 属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<question> questions
        {
            get
            {
                if ((_questions == null))
                {
                    _questions = base.CreateObjectSet<question>("questions");
                }
                return _questions;
            }
        }
        private ObjectSet<question> _questions;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<SuperUser> SuperUser
        {
            get
            {
                if ((_SuperUser == null))
                {
                    _SuperUser = base.CreateObjectSet<SuperUser>("SuperUser");
                }
                return _SuperUser;
            }
        }
        private ObjectSet<SuperUser> _SuperUser;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<report> Reports
        {
            get
            {
                if ((_Reports == null))
                {
                    _Reports = base.CreateObjectSet<report>("Reports");
                }
                return _Reports;
            }
        }
        private ObjectSet<report> _Reports;
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        public ObjectSet<student> Students
        {
            get
            {
                if ((_Students == null))
                {
                    _Students = base.CreateObjectSet<student>("Students");
                }
                return _Students;
            }
        }
        private ObjectSet<student> _Students;

        #endregion

        #region AddTo 方法
    
        /// <summary>
        /// 用于向 questions EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToquestions(question question)
        {
            base.AddObject("questions", question);
        }
    
        /// <summary>
        /// 用于向 SuperUser EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToSuperUser(SuperUser superUser)
        {
            base.AddObject("SuperUser", superUser);
        }
    
        /// <summary>
        /// 用于向 Reports EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToReports(report report)
        {
            base.AddObject("Reports", report);
        }
    
        /// <summary>
        /// 用于向 Students EntitySet 添加新对象的方法，已弃用。请考虑改用关联的 ObjectSet&lt;T&gt; 属性的 .Add 方法。
        /// </summary>
        public void AddToStudents(student student)
        {
            base.AddObject("Students", student);
        }

        #endregion

    }

    #endregion

    #region 实体
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AdminModel", Name="question")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class question : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 question 对象。
        /// </summary>
        /// <param name="id">Id 属性的初始值。</param>
        /// <param name="question1">Question 属性的初始值。</param>
        /// <param name="a">A 属性的初始值。</param>
        /// <param name="b">B 属性的初始值。</param>
        /// <param name="c">C 属性的初始值。</param>
        /// <param name="d">D 属性的初始值。</param>
        /// <param name="answers">Answers 属性的初始值。</param>
        public static question Createquestion(global::System.Int32 id, global::System.String question1, global::System.String a, global::System.String b, global::System.String c, global::System.String d, global::System.String answers)
        {
            question question = new question();
            question.Id = id;
            question.Question = question1;
            question.A = a;
            question.B = b;
            question.C = c;
            question.D = d;
            question.Answers = answers;
            return question;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Question
        {
            get
            {
                return _Question;
            }
            set
            {
                OnQuestionChanging(value);
                ReportPropertyChanging("Question");
                _Question = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Question");
                OnQuestionChanged();
            }
        }
        private global::System.String _Question;
        partial void OnQuestionChanging(global::System.String value);
        partial void OnQuestionChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String A
        {
            get
            {
                return _A;
            }
            set
            {
                OnAChanging(value);
                ReportPropertyChanging("A");
                _A = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("A");
                OnAChanged();
            }
        }
        private global::System.String _A;
        partial void OnAChanging(global::System.String value);
        partial void OnAChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String B
        {
            get
            {
                return _B;
            }
            set
            {
                OnBChanging(value);
                ReportPropertyChanging("B");
                _B = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("B");
                OnBChanged();
            }
        }
        private global::System.String _B;
        partial void OnBChanging(global::System.String value);
        partial void OnBChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String C
        {
            get
            {
                return _C;
            }
            set
            {
                OnCChanging(value);
                ReportPropertyChanging("C");
                _C = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("C");
                OnCChanged();
            }
        }
        private global::System.String _C;
        partial void OnCChanging(global::System.String value);
        partial void OnCChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String D
        {
            get
            {
                return _D;
            }
            set
            {
                OnDChanging(value);
                ReportPropertyChanging("D");
                _D = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("D");
                OnDChanged();
            }
        }
        private global::System.String _D;
        partial void OnDChanging(global::System.String value);
        partial void OnDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Answers
        {
            get
            {
                return _Answers;
            }
            set
            {
                OnAnswersChanging(value);
                ReportPropertyChanging("Answers");
                _Answers = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Answers");
                OnAnswersChanged();
            }
        }
        private global::System.String _Answers;
        partial void OnAnswersChanging(global::System.String value);
        partial void OnAnswersChanged();

        #endregion

    
        #region 导航属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AdminModel", "FK_Reports_questions", "Reports")]
        public EntityCollection<report> Reports
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<report>("AdminModel.FK_Reports_questions", "Reports");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<report>("AdminModel.FK_Reports_questions", "Reports", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AdminModel", Name="report")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class report : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 report 对象。
        /// </summary>
        /// <param name="id">Id 属性的初始值。</param>
        /// <param name="stuId">StuId 属性的初始值。</param>
        /// <param name="stuName">stuName 属性的初始值。</param>
        /// <param name="questionId">QuestionId 属性的初始值。</param>
        /// <param name="question">Question 属性的初始值。</param>
        /// <param name="a">A 属性的初始值。</param>
        /// <param name="b">B 属性的初始值。</param>
        /// <param name="c">C 属性的初始值。</param>
        /// <param name="d">D 属性的初始值。</param>
        /// <param name="correctAnswer">CorrectAnswer 属性的初始值。</param>
        public static report Createreport(global::System.Int32 id, global::System.Int32 stuId, global::System.String stuName, global::System.Int32 questionId, global::System.String question, global::System.String a, global::System.String b, global::System.String c, global::System.String d, global::System.String correctAnswer)
        {
            report report = new report();
            report.Id = id;
            report.StuId = stuId;
            report.stuName = stuName;
            report.QuestionId = questionId;
            report.Question = question;
            report.A = a;
            report.B = b;
            report.C = c;
            report.D = d;
            report.CorrectAnswer = correctAnswer;
            return report;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 StuId
        {
            get
            {
                return _StuId;
            }
            set
            {
                OnStuIdChanging(value);
                ReportPropertyChanging("StuId");
                _StuId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("StuId");
                OnStuIdChanged();
            }
        }
        private global::System.Int32 _StuId;
        partial void OnStuIdChanging(global::System.Int32 value);
        partial void OnStuIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String stuName
        {
            get
            {
                return _stuName;
            }
            set
            {
                OnstuNameChanging(value);
                ReportPropertyChanging("stuName");
                _stuName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("stuName");
                OnstuNameChanged();
            }
        }
        private global::System.String _stuName;
        partial void OnstuNameChanging(global::System.String value);
        partial void OnstuNameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 QuestionId
        {
            get
            {
                return _QuestionId;
            }
            set
            {
                OnQuestionIdChanging(value);
                ReportPropertyChanging("QuestionId");
                _QuestionId = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("QuestionId");
                OnQuestionIdChanged();
            }
        }
        private global::System.Int32 _QuestionId;
        partial void OnQuestionIdChanging(global::System.Int32 value);
        partial void OnQuestionIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Question
        {
            get
            {
                return _Question;
            }
            set
            {
                OnQuestionChanging(value);
                ReportPropertyChanging("Question");
                _Question = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Question");
                OnQuestionChanged();
            }
        }
        private global::System.String _Question;
        partial void OnQuestionChanging(global::System.String value);
        partial void OnQuestionChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String A
        {
            get
            {
                return _A;
            }
            set
            {
                OnAChanging(value);
                ReportPropertyChanging("A");
                _A = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("A");
                OnAChanged();
            }
        }
        private global::System.String _A;
        partial void OnAChanging(global::System.String value);
        partial void OnAChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String B
        {
            get
            {
                return _B;
            }
            set
            {
                OnBChanging(value);
                ReportPropertyChanging("B");
                _B = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("B");
                OnBChanged();
            }
        }
        private global::System.String _B;
        partial void OnBChanging(global::System.String value);
        partial void OnBChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String C
        {
            get
            {
                return _C;
            }
            set
            {
                OnCChanging(value);
                ReportPropertyChanging("C");
                _C = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("C");
                OnCChanged();
            }
        }
        private global::System.String _C;
        partial void OnCChanging(global::System.String value);
        partial void OnCChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String D
        {
            get
            {
                return _D;
            }
            set
            {
                OnDChanging(value);
                ReportPropertyChanging("D");
                _D = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("D");
                OnDChanged();
            }
        }
        private global::System.String _D;
        partial void OnDChanging(global::System.String value);
        partial void OnDChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CorrectAnswer
        {
            get
            {
                return _CorrectAnswer;
            }
            set
            {
                OnCorrectAnswerChanging(value);
                ReportPropertyChanging("CorrectAnswer");
                _CorrectAnswer = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("CorrectAnswer");
                OnCorrectAnswerChanged();
            }
        }
        private global::System.String _CorrectAnswer;
        partial void OnCorrectAnswerChanging(global::System.String value);
        partial void OnCorrectAnswerChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String UserAnswer
        {
            get
            {
                return _UserAnswer;
            }
            set
            {
                OnUserAnswerChanging(value);
                ReportPropertyChanging("UserAnswer");
                _UserAnswer = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("UserAnswer");
                OnUserAnswerChanged();
            }
        }
        private global::System.String _UserAnswer;
        partial void OnUserAnswerChanging(global::System.String value);
        partial void OnUserAnswerChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Double> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                OnGradeChanging(value);
                ReportPropertyChanging("Grade");
                _Grade = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Grade");
                OnGradeChanged();
            }
        }
        private Nullable<global::System.Double> _Grade;
        partial void OnGradeChanging(Nullable<global::System.Double> value);
        partial void OnGradeChanged();

        #endregion

    
        #region 导航属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AdminModel", "FK_Reports_questions", "question")]
        public question questions
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<question>("AdminModel.FK_Reports_questions", "question").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<question>("AdminModel.FK_Reports_questions", "question").Value = value;
            }
        }
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<question> questionsReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<question>("AdminModel.FK_Reports_questions", "question");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<question>("AdminModel.FK_Reports_questions", "question", value);
                }
            }
        }
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AdminModel", "FK_Reports_Students", "Students")]
        public student Students
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<student>("AdminModel.FK_Reports_Students", "Students").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<student>("AdminModel.FK_Reports_Students", "Students").Value = value;
            }
        }
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<student> StudentsReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<student>("AdminModel.FK_Reports_Students", "Students");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<student>("AdminModel.FK_Reports_Students", "Students", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AdminModel", Name="student")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class student : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 student 对象。
        /// </summary>
        /// <param name="stuId">stuId 属性的初始值。</param>
        /// <param name="userName">userName 属性的初始值。</param>
        /// <param name="passWord">passWord 属性的初始值。</param>
        public static student Createstudent(global::System.Int32 stuId, global::System.String userName, global::System.String passWord)
        {
            student student = new student();
            student.stuId = stuId;
            student.userName = userName;
            student.passWord = passWord;
            return student;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 stuId
        {
            get
            {
                return _stuId;
            }
            set
            {
                if (_stuId != value)
                {
                    OnstuIdChanging(value);
                    ReportPropertyChanging("stuId");
                    _stuId = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("stuId");
                    OnstuIdChanged();
                }
            }
        }
        private global::System.Int32 _stuId;
        partial void OnstuIdChanging(global::System.Int32 value);
        partial void OnstuIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String userName
        {
            get
            {
                return _userName;
            }
            set
            {
                OnuserNameChanging(value);
                ReportPropertyChanging("userName");
                _userName = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("userName");
                OnuserNameChanged();
            }
        }
        private global::System.String _userName;
        partial void OnuserNameChanging(global::System.String value);
        partial void OnuserNameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String passWord
        {
            get
            {
                return _passWord;
            }
            set
            {
                OnpassWordChanging(value);
                ReportPropertyChanging("passWord");
                _passWord = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("passWord");
                OnpassWordChanged();
            }
        }
        private global::System.String _passWord;
        partial void OnpassWordChanging(global::System.String value);
        partial void OnpassWordChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Double> baseGrade
        {
            get
            {
                return _baseGrade;
            }
            set
            {
                OnbaseGradeChanging(value);
                ReportPropertyChanging("baseGrade");
                _baseGrade = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("baseGrade");
                OnbaseGradeChanged();
            }
        }
        private Nullable<global::System.Double> _baseGrade;
        partial void OnbaseGradeChanging(Nullable<global::System.Double> value);
        partial void OnbaseGradeChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Double> Grade
        {
            get
            {
                return _Grade;
            }
            set
            {
                OnGradeChanging(value);
                ReportPropertyChanging("Grade");
                _Grade = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Grade");
                OnGradeChanged();
            }
        }
        private Nullable<global::System.Double> _Grade;
        partial void OnGradeChanging(Nullable<global::System.Double> value);
        partial void OnGradeChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Double> Grade1
        {
            get
            {
                return _Grade1;
            }
            set
            {
                OnGrade1Changing(value);
                ReportPropertyChanging("Grade1");
                _Grade1 = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Grade1");
                OnGrade1Changed();
            }
        }
        private Nullable<global::System.Double> _Grade1;
        partial void OnGrade1Changing(Nullable<global::System.Double> value);
        partial void OnGrade1Changed();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Double> Grade2
        {
            get
            {
                return _Grade2;
            }
            set
            {
                OnGrade2Changing(value);
                ReportPropertyChanging("Grade2");
                _Grade2 = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Grade2");
                OnGrade2Changed();
            }
        }
        private Nullable<global::System.Double> _Grade2;
        partial void OnGrade2Changing(Nullable<global::System.Double> value);
        partial void OnGrade2Changed();

        #endregion

    
        #region 导航属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("AdminModel", "FK_Reports_Students", "report")]
        public EntityCollection<report> Reports
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<report>("AdminModel.FK_Reports_Students", "report");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<report>("AdminModel.FK_Reports_Students", "report", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// 没有元数据文档可用。
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="AdminModel", Name="SuperUser")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class SuperUser : EntityObject
    {
        #region 工厂方法
    
        /// <summary>
        /// 创建新的 SuperUser 对象。
        /// </summary>
        /// <param name="id">Id 属性的初始值。</param>
        /// <param name="name">Name 属性的初始值。</param>
        /// <param name="password">Password 属性的初始值。</param>
        public static SuperUser CreateSuperUser(global::System.Int32 id, global::System.String name, global::System.String password)
        {
            SuperUser superUser = new SuperUser();
            superUser.Id = id;
            superUser.Name = name;
            superUser.Password = password;
            return superUser;
        }

        #endregion

        #region 基元属性
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Password
        {
            get
            {
                return _Password;
            }
            set
            {
                OnPasswordChanging(value);
                ReportPropertyChanging("Password");
                _Password = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Password");
                OnPasswordChanged();
            }
        }
        private global::System.String _Password;
        partial void OnPasswordChanging(global::System.String value);
        partial void OnPasswordChanged();

        #endregion

    
    }

    #endregion

    
}