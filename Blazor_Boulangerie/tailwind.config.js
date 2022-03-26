const colors = require('tailwindcss/colors');
module.exports = {
    content: [],
    purge: {
        enabled: true,
        content: [
            './**/*.html',
            './**/*.razor'
        ],
    },
    theme: {
        extend: {},
    },
    plugins: [],
}
