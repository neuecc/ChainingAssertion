Replace the initialization of RootVisual in Application_Startup with the following line:

this.StartTestRunnerDelayed();

If you would like to start the test immediately, use this form instead:

this.StartTestRunnerImmediate();

Please note that there is a known defect which causes the immediate form to run all tests twice.
