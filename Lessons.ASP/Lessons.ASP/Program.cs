
using System.Threading;
using System.Threading.Tasks;

namespace Lesson.ASP;

public static class Program
{
    public static void Main(string[] Args)
    {
        //устанавливает мксимальное кол-во потоков.
        //в скобках
        //кол-во потоков в пуле, кол-во потоков для операций ввода/вывода)
        ThreadPool.SetMaxThreads(10, 10);   //устанавливать ", 10)" не ракомендуется.

        //содержит некоторе кол-во потоков, которые выделяет для выполнения задач
        //по окончании выполнения поток возвращается в пул
        ThreadPool.QueueUserWorkItem(parameter =>
        {

        });
    }
}