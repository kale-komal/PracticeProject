using PracticeProject.Models;
using Microsoft.Data.SqlClient;
using PracticeProject.Data;
using System.Collections.Generic;


namespace PracticeProject.Data
{
    public class ProjectData : DataAccess
    {
        public ProjectData(IConfiguration configuration) : base(configuration)
        {
        }

        // Get All Projects
        public List<Projects> GetProjects()
        {
            var projects = new List<Projects>();

            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("SELECT * FROM Projects", con);
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        projects.Add(new Projects
                        {
                            ProjectId = reader["ProjectId"] != DBNull.Value ? Convert.ToInt32(reader["ProjectId"]) : 0,
                            ProjectName = reader["ProjectName"] != DBNull.Value ? reader["ProjectName"].ToString() : string.Empty,
                            ProjectDate = reader["ProjectDate"] != DBNull.Value ? Convert.ToDateTime(reader["ProjectDate"]) : DateTime.MinValue,
                            ProjectDesc = reader["ProjectDesc"] != DBNull.Value ? reader["ProjectDesc"].ToString() : string.Empty,
                            ProjectImage = reader["ProjectImage"] != DBNull.Value ? reader["ProjectImage"].ToString() : string.Empty
                        });
                    }
                }
            }
            return projects;
        }

        // Create a New Project

        public void AddProject(Projects project)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Projects (ProjectName, ProjectDate, ProjectDesc, ProjectImage) VALUES (@ProjectName, @ProjectDate, @ProjectDesc, @ProjectImage)";
                var cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                cmd.Parameters.AddWithValue("@ProjectDate", project.ProjectDate);
                cmd.Parameters.AddWithValue("@ProjectDesc", project.ProjectDesc);
                cmd.Parameters.AddWithValue("@ProjectImage", project.ProjectImage ?? (object)DBNull.Value);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected == 0)
                {
                    throw new Exception("Project not inserted into database.");
                }
            }
        }

        // Update Projects
        public void UpdateProject(Projects project)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE Projects SET ProjectName = @ProjectName, ProjectDate = @ProjectDate, ProjectDesc = @ProjectDesc, ProjectImage = @ProjectImage WHERE ProjectId = @ProjectId", con))
            {
                cmd.Parameters.AddWithValue("@ProjectId", project.ProjectId);
                cmd.Parameters.AddWithValue("@ProjectName", project.ProjectName);
                cmd.Parameters.AddWithValue("@ProjectDate", project.ProjectDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ProjectDesc", project.ProjectDesc);
                cmd.Parameters.AddWithValue("@ProjectImage", project.ProjectImage ?? (object)DBNull.Value);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
        }


        // Get single product by id

        public Projects? GetProjectsById(int id)
        {
            using (var con = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM Projects WHERE ProjectId = @ProjectId", con))
            {
                cmd.Parameters.AddWithValue("@ProjectId", id);
                con.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        return new Projects
                        {
                            ProjectId = Convert.ToInt32(reader["ProjectId"]),
                            ProjectName = reader["ProjectName"].ToString(),
                            ProjectDate = reader["ProjectDate"] != DBNull.Value ? Convert.ToDateTime(reader["ProjectDate"]) : DateTime.Now,
                            ProjectDesc = reader["ProjectDesc"]?.ToString() ?? string.Empty,
                            ProjectImage = reader["ProjectImage"]?.ToString() ?? string.Empty
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        // Delete Projects

        public void DeleteProject(int ProjectId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("DELETE FROM Projects WHERE ProjectId = @ProjectId", con);
                cmd.Parameters.AddWithValue("@ProjectId", ProjectId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

    }
}
