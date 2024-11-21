// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");

module.exports = tseslint.config(
    {
        files: ["**/*.ts"],
        extends: [
            eslint.configs.recommended,
            ...tseslint.configs.recommended,
            ...tseslint.configs.stylistic,
            ...angular.configs.tsRecommended
        ],
        processor: angular.processInlineTemplates,
        rules: {
            "@angular-eslint/directive-selector": [
                "error",
                {
                    type: "attribute",
                    prefix: "app",
                    style: "camelCase"
                }
            ],
            "@angular-eslint/component-selector": [
                "error",
                {
                    type: "element",
                    prefix: "app",
                    style: "kebab-case"
                }
            ],
            "@angular-eslint/no-output-on-prefix": "off",
            "@typescript-eslint/no-explicit-any": "off",
            "@angular-eslint/no-output-native": "off"
        }
    },
    {
        files: ["**/*.html"],
        extends: [
            ...angular.configs.templateRecommended,
            ...angular.configs.templateAccessibility
        ],
        rules: {
            "@angular-eslint/template/elements-content": "off"
        }
    },
    {
        files: ["src/guards/auth.guard.ts", "src/guards/no-auth.guard.ts"],
        rules: {
            "@typescript-eslint/no-unused-vars": "off"
        }
    }
);
