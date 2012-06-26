using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interfaces
{
    //*********************************
    // типа это мне и надо от вас
    //***********************************
    public interface IBot
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

        /// <summary>
        /// Инициализация бота - передача ссылки на класс API(бывш. MasterChif)
        /// </summary>
        /// <param name="api">Ссылка на объект, реализующий API для использования ботом</param>
        void Initialize(IAPI api);

    }
}
