using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ByteDev.Testing
{
    /// <summary>
    /// Represents a method to retrieve a test connection
    /// string to a resource.
    /// </summary>
    public class TestConnectionString
    {
        /// <summary>
        /// Environment variable name containing the connection string.
        /// </summary>
        public string EnvironmentVarName { get; set; }

        /// <summary>
        /// Text file paths that could contain the connection string. By default
        /// will contain a number of possible file paths.
        /// </summary>
        public IEnumerable<string> FilePaths { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Testing.TestConnectionString" /> class.
        /// </summary>
        /// <param name="containingAssembly">Containing test assembly that is consuming the connection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containingAssembly" /> is null.</exception>
        public TestConnectionString(Assembly containingAssembly)
        {
            if (containingAssembly == null)
                throw new ArgumentNullException(nameof(containingAssembly));

            FilePaths = DefaultFilePaths.GetDefaultFilePaths(DefaultFileName.GetDefaultConnStringFileName(containingAssembly));
        }

        /// <summary>
        /// Retrieves the test connection string. First will try and retrieve from environment variable
        /// (process, user, machine). Second from a text file containing the connection string.
        /// </summary>
        /// <returns>Connection string if found; otherwise throws exception.</returns>
        /// <exception cref="T:ByteDev.Testing.TestingException">"Could not find test connection string.</exception>
        public string GetConnectionString()
        {
            var connStr = GetConnStringFromEnvVar();

            if (!string.IsNullOrEmpty(connStr))
                return connStr;

            connStr = GetConnStringFromFile();

            if (!string.IsNullOrEmpty(connStr))
                return connStr;

            throw new TestingException("Could not find test connection string.");
        }

        private string GetConnStringFromEnvVar()
        {
            if (string.IsNullOrEmpty(EnvironmentVarName))
                return null;
            
            var value = Environment.GetEnvironmentVariable(EnvironmentVarName, EnvironmentVariableTarget.Process);

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(EnvironmentVarName, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(value))
                value = Environment.GetEnvironmentVariable(EnvironmentVarName, EnvironmentVariableTarget.Machine);

            return value;
        }

        private string GetConnStringFromFile()
        {
            if (FilePaths == null)
                return null;

            foreach (var filePath in FilePaths)
            {
                if (File.Exists(filePath))
                    return File.ReadAllText(filePath).Trim();
            }

            return null;
        }
    }
}