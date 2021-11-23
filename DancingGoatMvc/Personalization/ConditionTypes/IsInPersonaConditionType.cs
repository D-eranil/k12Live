﻿using System;

using CMS.ContactManagement;
using CMS.Personas;

using DancingGoat.Controllers.Personalization;
using DancingGoat.Personalization;

using Kentico.PageBuilder.Web.Mvc.Personalization;

using Newtonsoft.Json;

[assembly: RegisterPersonalizationConditionType("DancingGoat.Personalization.IsInPersona", typeof(IsInPersonaConditionType), "{$DancingGoatMvc.ConditionType.IsInPersona.Name$}", ControllerType = typeof(IsInPersonaController), Description = "{$DancingGoatMvc.ConditionType.IsInPersona.Description$}", Hint = "{$DancingGoatMvc.ConditionType.IsInPersona.Hint$}", IconClass = "icon-app-personas")]

namespace DancingGoat.Personalization
{
    /// <summary>
    /// Personalization condition type based on persona.
    /// </summary>
    public class IsInPersonaConditionType : ConditionType
    {
        /// <summary>
        /// Selected persona code name.
        /// </summary>
        public string PersonaName { get; set; }


        /// <summary>
        /// Automatically populated variant name.
        /// </summary>
        public override string VariantName
        {
            get
            {
                return Persona?.PersonaDisplayName;
            }
            set
            {
                // No need to set automatically generated variant name
            }
        }


        private readonly Lazy<PersonaInfo> mPersona;


        [JsonIgnore]
        private PersonaInfo Persona => mPersona.Value;


        /// <summary>
        /// Creates an empty instance of <see cref="IsInPersonaConditionType"/> class.
        /// </summary>
        public IsInPersonaConditionType()
        {
            mPersona = new Lazy<PersonaInfo>(() => PersonaInfoProvider.GetPersonaInfoByCodeName(PersonaName));
        }


        /// <summary>
        /// Evaluate condition type.
        /// </summary>
        /// <returns>Returns <c>true</c> if implemented condition is met.</returns>
        public override bool Evaluate()
        {
            if (Persona == null)
            {
                return false;
            }

            var contact = ContactManagementContext.GetCurrentContact(false);

            return contact?.ContactPersonaID == Persona.PersonaID;
        }
    }
}