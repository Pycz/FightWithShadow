using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasterElement
{
    //*********************************
    // типа это мне и надо от вас
    //***********************************
    interface IBot
    {

        ///<summary>Сходить боту - public</summary>
        ///<returns>Куда и как сходить</returns>
        IStep doSmth();

        /// 2.  ?????????????

        /// 3.  Возврат имени бота, не более 14 символов
        //  14 символов хватит всем, лол
        ///<summary>Отдать имя бота - public</summary>
        ///<returns>Имя или фамилия или ник или отчество или наименование или прозвище или кличка или еще какая фигня</returns>
        string getName();

    }
}
