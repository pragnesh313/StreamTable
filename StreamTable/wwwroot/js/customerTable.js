import { streamService } from './streamService.js';
const { scan, throttleTime } = rxjs.operators;

const initCustomerTable = (selector) => {
    const table = $(selector).DataTable({
        columns: [
            { data: "id" },
            { data: "firstName" },
            { data: "lastName" },
            { data: "contact" },
            { data: "email" },
            {
                data: "dateOfBirth",
                render: (data) => data ? new Date(data).toLocaleDateString() : ''
            },
            {
                render: (data, type, row) =>
                    `<a href='#' class='btn btn-sm btn-primary' onclick="Edit('${row.id}')">Edit</a>`
            },
        ],
        order: [[5, 'desc']],
        deferRender: true 
    });

    // With sql statement
    //streamService.getStream('/api/customer')
    // With sql query
    streamService.getStream('/api/customer/sql')
        .pipe(
            scan((acc, newRows) => [...acc, ...newRows], []),
            throttleTime(100, rxjs.asyncScheduler, { leading: true, trailing: true })
        )
        .subscribe({
            next: (allRows) => {
                table.clear();
                table.rows.add(allRows);
                table.draw(false);
            },
            error: (err) => console.error("Table Stream Error:", err)
        });
};

$(document).ready(() => initCustomerTable("#customerTable"));