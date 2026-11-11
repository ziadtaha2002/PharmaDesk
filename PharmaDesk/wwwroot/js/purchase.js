let rowIndex = 0;

document.addEventListener('DOMContentLoaded', function () {
    // تفعيل البحث في القوائم المنسدلة (Select2)
    initializeSelect2();

    // إذا رجع السيرفر بيانات (بسبب خطأ)، أعد بناء الصفوف
    if (existingItems && existingItems.length > 0) {
        existingItems.forEach(item => {
            addRow(item);
        });
    } else {
        // إذا كانت صفحة جديدة، أضف صفاً واحداً فارغاً
        addRow();
    }
});

function initializeSelect2() {
    $('.searchable-select').select2({
        theme: 'bootstrap-5', // <-- استخدام الثيم الجديد
        placeholder: "-- Choose a supplier --",
        allowClear: true,
        width: '100%',
        dropdownParent: $('#purchaseForm')
    });
}

function addRow(data = null) {
    const template = document.getElementById('rowTemplate');
    if (!template) return;

    const html = template.innerHTML.replace(/__INDEX__/g, rowIndex);
    const tbody = document.getElementById('itemsBody');
    tbody.insertAdjacentHTML('beforeend', html);

    const newRow = tbody.lastElementChild;

    $(newRow).find('.searchable-select-medicine').select2({
        theme: 'bootstrap-5', // <-- استخدام الثيم الجديد
        placeholder: "-- Choose --",
        allowClear: true,
        width: '100%',
        dropdownParent: $('#itemsTable')
    });

    if (data) {
        $(newRow).find('.searchable-select-medicine').val(data.MedicineId).trigger('change');
        $(newRow).find('input[name*="BatchNumber"]').val(data.BatchNumber);
        $(newRow).find('input[name*="ExpiryDate"]').val(data.ExpiryDate);
        $(newRow).find('input[name*="Quantity"]').val(data.Quantity);
        $(newRow).find('input[name*="CostPrice"]').val(data.CostPrice);
    }

    rowIndex++;
    bindEvents();
    calculateGrandTotal();
}

function removeRow(button) {
    const row = button.closest('tr');
    row.remove();
    calculateGrandTotal();
}

function bindEvents() {
    document.querySelectorAll('.qty-input, .price-input').forEach(input => {
        input.removeEventListener('input', onInputChange);
        input.addEventListener('input', onInputChange);
    });
}

function onInputChange(event) {
    const row = event.target.closest('tr');
    const qty = parseFloat(row.querySelector('.qty-input').value) || 0;
    const price = parseFloat(row.querySelector('.price-input').value) || 0;
    
    row.querySelector('.line-total').textContent = '$' + (qty * price).toFixed(2);
    calculateGrandTotal();
}

function calculateGrandTotal() {
    let total = 0;
    document.querySelectorAll('#itemsBody tr').forEach(row => {
        const qty = parseFloat(row.querySelector('.qty-input').value) || 0;
        const price = parseFloat(row.querySelector('.price-input').value) || 0;
        total += (qty * price);
    });
    document.getElementById('grandTotal').textContent = total.toFixed(2);
}