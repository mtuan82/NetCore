using Microsoft.AspNetCore.Authorization;

namespace Core.Authorization
{
    public class HasScopeRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets the issuers.
        /// </summary>
        /// <value>The issuers.</value>
        public List<string> Issuers { get; }
        /// <summary>
        /// Gets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public string Scope { get; }
        /// <summary>
        /// Gets the scope base domain.
        /// </summary>
        /// <value>The scope base domain.</value>
        public string ScopeBaseDomain { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasScopeRequirement" /> class.
        /// </summary>
        /// <param name="scopeBaseDomain">The scope's base domain.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="issuers">The issuers.</param>
        /// <exception cref="ArgumentNullException">scopeBaseDomain
        /// or
        /// scope
        /// or
        /// issuer</exception>
        public HasScopeRequirement(string scopeBaseDomain, string scope, List<string> issuers)
        {
            ScopeBaseDomain = scopeBaseDomain ?? throw new ArgumentNullException(nameof(scopeBaseDomain));
            Scope = scope ?? throw new ArgumentNullException(nameof(scope));
            Issuers = issuers ?? throw new ArgumentNullException(nameof(issuers));
        }
    }
}
