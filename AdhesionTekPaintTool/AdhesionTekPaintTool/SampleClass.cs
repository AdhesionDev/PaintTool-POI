using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdhesionTekPaintTool
{
    class SampleClass
    {
        #region Singleton

        /// <summary>
        /// The singleton of the SampleClass
        /// </summary>
        public static SampleClass Instance;

        #endregion

        #region Private Field

        private float _sampleVar;

        #endregion

        #region Public Field

        public float sampleVar
        {
            get
            {
                return _sampleVar;
            }
        }

        #endregion

        /// <summary>
        /// The constructor of sample class.
        /// </summary>
        /// <param name="sampleVar">The sample var passed into the constructor.</param>
        public SampleClass(float sampleVar)
        {
            // Create a singleton of the sample class
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new InvalidOperationException("Singleton already exist.");
            }

            this._sampleVar = sampleVar;
        }
    }
}
