
using System.Threading;
using System.Threading.Tasks;

namespace Lesson.ASP;

public static class Program
{
    public static void Main(string[] Args)
    {
        var list = new List<string>();

        //"флаги" позволяющие сделать одновременный старт потоков.
        var manual_event = new ManualResetEvent(false);
        var _auto_event = new AutoResetEvent(false);    //разрешает запуск только одного потока за один .Set();

        ////спомощью Mutex можно отслеживать единственность запуска программы.
        ////если программа запущена не первой, id_First вернет false
        //Mutex mutex = new Mutex(true, "MySingleton program.", out var id_First);
        ////при завершении работы программы его нужно уничтожить
        //mutex.ReleaseMutex();

        ////позволяет пропускать определенное кол-во потоков. Тот же Mutex только с указание кол-ва потоков.
        //Semaphore semaphore = new Semaphore(1, 1);
        //semaphore.WaitOne();    //точка ожидания потоков
        //semaphore.Release();    //уничтожение semaphore



        var threads = new Thread[10];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                var thread_id = Thread.CurrentThread.ManagedThreadId;
                Console.WriteLine("Поток {0} создан и ждет разрешения.", thread_id);

                //тут потоки остановятся
                manual_event.WaitOne();
                Console.WriteLine("Поток {0} запущен.", thread_id);

                for (int j = 0; j < 10; j++)
                {
                    list.Add($"Thread value {j} from thread id: {thread_id}");
                }

                Console.WriteLine("Поток {0} завершен.", thread_id);
            });

            threads[i].Start();
        }

        Console.WriteLine("Все потоки запуены и готовы к работе.");
        Console.ReadLine();

        //после этого все потоки начнут свою работу после строки manual_event.WaitOne()
        manual_event.Set();
        Console.WriteLine("Потокам разрешено выполнить работу.");

        Console.ReadLine();
    }
}