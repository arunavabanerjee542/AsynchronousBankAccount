using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;

namespace AsynchronousBankAccount
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SampleDataContext db = new SampleDataContext();
            JointAccount j =  db.JointAccounts.Where(x => x.id == 1).Single();

          

            try
            {
                // User 1 tries to deposit 500 to the account
                await deposit(500, j, db);



                // user 2 tries to withdraw 200 at the same time when user 1 is depositng
                db.ExecuteCommand("Update JointAccount set balance = balance - 200 where id=1");



                // change conflict exception occurs 
                db.SubmitChanges();



            }

            catch(Exception e)
            {

                db.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);  // preference will only be given to user 1 actions
                db.SubmitChanges();

            }

            Console.WriteLine(j.balance);
           

        }





        public static async Task deposit(int amount , JointAccount j,SampleDataContext db)
        {
            j.balance += amount;    

            await Task.Delay(2000);

            

            
        }

      



    }
}
