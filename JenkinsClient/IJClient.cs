namespace JenkinsClient
{
    public interface IJClient
    {
        Jobs GetJobs();
        Builds GetBuilds(string jobname, int take = 5);
        bool Test();
    }
}
