﻿using System;
using KRPC.Service;
using KRPC.Schema.Control;
using KSP;

namespace KRPCServices
{
    [KRPCService]
    public class Control
    {
        [KRPCProcedure]
        public static void EnableSAS ()
        {
            FlightGlobals.ActiveVessel.ActionGroups.SetGroup (KSPActionGroup.SAS, true);
        }

        [KRPCProcedure]
        public static void DisableSAS ()
        {
            FlightGlobals.ActiveVessel.ActionGroups.SetGroup (KSPActionGroup.SAS, false);
        }

        [KRPCProcedure]
        public static void EnableRCS ()
        {
            FlightGlobals.ActiveVessel.ActionGroups.SetGroup (KSPActionGroup.RCS, true);
        }

        [KRPCProcedure]
        public static void DisableRCS ()
        {
            FlightGlobals.ActiveVessel.ActionGroups.SetGroup (KSPActionGroup.RCS, false);
        }

        [KRPCProcedure]
        public static void SetThrottle (Throttle throttle)
        {
            FlightInputHandler.state.mainThrottle = throttle.Throttle_;
        }

        [KRPCProcedure]
        public static Throttle GetThrottle ()
        {
            return Throttle.CreateBuilder ().SetThrottle_ (FlightInputHandler.state.mainThrottle).Build ();
        }

        [KRPCProcedure]
        public static void SetControlInputs(ControlInputs controls) {
            PilotAddon.SetControlInputs (controls);
        }

        [KRPCProcedure]
        public static void ActivateNextStage() {
            Staging.ActivateNextStage();
        }
    }
}
