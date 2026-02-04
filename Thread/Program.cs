namespace Thread;

class Program
{
    static void Main(string[] args)
    {
        int num = 0;
        
        object obj = new object();

        System.Threading.Thread thread1 = new System.Threading.Thread(() =>
        {
            for (int i = 0; i < 1000000; i++)
            {
                lock (obj)
                {
                    num++;
                }
                // 여러 스레드가 동시에 동일한 변수를 접근해서 문제가 생기는 영역을 크리티컬 액션
                // 한 순간에 두 개 이상의 스레드가 접근 할 수 없도록 막아야 하는데 스레드 동기화
                // 1. 메모리에서 0을 가져오고
                // 2. 1을 더하고
                // 3. 메모리에 1을 저장
            }
        });
        thread1.Start();

        System.Threading.Thread thread2 = new System.Threading.Thread(() =>
        {
            for (int i = 0; i < 1000000; i++)
            {
                lock (obj)
                {
                    num++;
                }
                // 1. 메모리에서 0을 가져오고
                // 2. 1을 더하고
                // 3. 메모리에 1을 저장
            }
        });
        thread2.Start();
        
        thread1.Join();
        thread2.Join();
        Console.WriteLine(num);
    }
}