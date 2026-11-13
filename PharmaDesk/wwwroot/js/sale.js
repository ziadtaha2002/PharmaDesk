let cart = [];

// التركيز التلقائي على الباركود عند فتح الصفحة
document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('barcodeInput').focus();
});

// عند الضغط على Enter في مربع الباركود
document.getElementById('barcodeInput').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        const query = this.value.trim();
        if (query === '') return;

        // البحث عن الدواء بالباركود أولاً
        let medicine = allMedicines.find(m => m.Barcode === query);

        // إذا لم يوجد بالباركود، ابحث بالاسم
        if (!medicine) {
            const matches = allMedicines.filter(m => m.Name.toLowerCase().includes(query.toLowerCase()));
            if (matches.length === 1) {
                medicine = matches[0];
            } else if (matches.length > 1) {
                // إذا كان هناك أكثر من نتيجة، افتح نافذة البحث
                openSearchModal(query);
                this.value = '';
                return;
            }
        }

        if (medicine) {
            addToCart(medicine);
            this.value = ''; // مسح المربع للاستعداد للباركود التالي
        } else {
            alert('Medicine not found!');
        }
    }
});

// إضافة دواء للسلة
function addToCart(medicine) {
    const existingItem = cart.find(item => item.MedicineId === medicine.Id);
    
    if (existingItem) {
        existingItem.Quantity++;
    } else {
        cart.push({
            MedicineId: medicine.Id,
            MedicineName: medicine.Name,
            UnitPrice: medicine.SellingPrice,
            Quantity: 1
        });
    }
    
    renderCart();
}

// رسم السلة على الشاشة
function renderCart() {
    const tbody = document.getElementById('cartBody');
    
    if (cart.length === 0) {
        tbody.innerHTML = '<tr id="emptyCartRow"><td colspan="5" class="text-center text-muted py-4">Cart is empty.</td></tr>';
        updateSummary();
        return;
    }

    tbody.innerHTML = cart.map((item, index) => `
        <tr>
            <td>${item.MedicineName}</td>
            <td>
                <input type="number" min="1" value="${item.Quantity}" 
                       class="form-control form-control-sm" 
                       onchange="updateQuantity(${index}, this.value)" />
            </td>
            <td class="text-end">$${item.UnitPrice.toFixed(2)}</td>
            <td class="text-end">$${(item.Quantity * item.UnitPrice).toFixed(2)}</td>
            <td>
                <button class="btn btn-sm btn-outline-danger" onclick="removeItem(${index})">
                    <i class="bi bi-x"></i>
                </button>
            </td>
        </tr>
    `).join('');

    updateSummary();
}

// تحديث الكمية
function updateQuantity(index, newQty) {
    const qty = parseInt(newQty);
    if (qty > 0) {
        cart[index].Quantity = qty;
    } else {
        cart[index].Quantity = 1;
    }
    renderCart();
}

// حذف دواء من السلة
function removeItem(index) {
    cart.splice(index, 1);
    renderCart();
}

// تفريغ السلة بالكامل
function clearCart() {
    if (confirm('Are you sure you want to clear the cart?')) {
        cart = [];
        renderCart();
        document.getElementById('barcodeInput').focus();
    }
}

// تحديث الملخص (الإجمالي وعدد القطع)
function updateSummary() {
    const totalItems = cart.reduce((sum, item) => sum + item.Quantity, 0);
    const totalAmount = cart.reduce((sum, item) => sum + (item.Quantity * item.UnitPrice), 0);
    
    document.getElementById('summaryItems').textContent = totalItems;
    document.getElementById('summaryTotal').textContent = '$' + totalAmount.toFixed(2);
}

// حفظ الفاتورة وإرسالها للسيرفر
async function saveSale() {
    if (cart.length === 0) {
        alert('Cart is empty!');
        return;
    }

    // جلب رمز الحماية
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    
    if (!token) {
        alert("Security token missing! Please refresh the page.");
        return;
    }

    const payload = {
        PaymentMethod: "Cash",
        Items: cart.map(item => ({
            MedicineId: item.MedicineId,
            Quantity: item.Quantity
        }))
    };

    try {
        // إرسال البيانات
        const response = await fetch('/Sale/Save', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(payload)
        });

        // التحقق من استجابة السيرفر
        if (!response.ok) {
            const text = await response.text();
            console.error("Server Error:", text);
            alert("Server error! Check console (F12).");
            return;
        }

        const result = await response.json();

        if (result.success) {
            // فتح الإيصال
            window.open('/Sale/Receipt/' + result.saleId, '_blank');
            // تفريغ السلة
            cart = [];
            renderCart();
            document.getElementById('barcodeInput').focus();
        } else {
            alert('Error: ' + result.message);
        }
    } catch (error) {
        console.error("Fetch Error:", error);
        alert("Network error! Check console (F12).");
    }
}

// نافذة البحث (إذا كان هناك أدوية متعددة بنفس الاسم)
function openSearchModal(query) {
    const modal = new bootstrap.Modal(document.getElementById('searchModal'));
    const resultsDiv = document.getElementById('modalSearchResults');
    
    const matches = allMedicines.filter(m => m.Name.toLowerCase().includes(query.toLowerCase()));
    
    resultsDiv.innerHTML = matches.map(m => `
        <button class="list-group-item list-group-item-action" 
                onclick="selectFromModal(${m.Id})">
            <strong>${m.Name}</strong> - $${m.SellingPrice.toFixed(2)}
        </button>
    `).join('');

    modal.show();
}

function selectFromModal(medicineId) {
    const medicine = allMedicines.find(m => m.Id === medicineId);
    if (medicine) {
        addToCart(medicine);
    }
    const modalEl = document.getElementById('searchModal');
    const modal = bootstrap.Modal.getInstance(modalEl);
    modal.hide();
    document.getElementById('barcodeInput').focus();
}