using System;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

/*
Allows us to fire off a function with no parameters or returns after some amount of time, in milleseconds. 
 */
namespace CosmicWatch_Library
{
    public class DelayedEvent
    {
        /*
         Function to execute at the end of the timer.
         */
        public delegate void endFunction();
        public endFunction Function;

        /*
         The timer object which enables this.
         */
        Timer timer;

        /*
         Constructor
         
         Parameters:
            long timeFor: Number of milleseconds to wait before firing off the function.
            endFunction function: The void function() to execute at the end of the timer.

         Note: The timer automatically begins upon construction; there is NO need to call start!
            
         */
        public DelayedEvent(long timeFor, endFunction function)
        {
            if (timeFor < 1) { timeFor = 1; } //Prevent people from initializing the timer with 0 or less. Todo: This is a hack.
            timer = new Timer(timeFor);
            timer.Elapsed += EndEvent;
            this.Function += function;
            Start();
            //Testing purposes.
            //Terminate();
        }

        //Action when the time is up.

        /*
         Upon ending the timer, do these things.
         */
        public void EndEvent(Object source, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Function?.Invoke(); 
                timer.Stop();
            });
        }

        //Starting and preemptively terminating the delayed event.

        /*
         Start the timer.
        
         Note: The timer automatically begins upon construction; there is NO need to call start! Do it only if you've called stop().
         */
        public void Start()
        {
            timer.Start();
        }
        /*
         Stop the timer (and prevent the event from executing if the event hasn't executed).
         */
        public void Terminate()
        {
            timer.Stop();
        }
        /*
         End the timer early aand fire the event.
         */
        public void EndEarly()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Function?.Invoke();
                timer.Stop();
            });
        }
    }
}
