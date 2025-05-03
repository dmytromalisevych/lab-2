// Файл script.js - покладіть його у папку wwwroot/js

// Функції для обробки кнопок
function handleDetailsClick(appointmentId, patientName, doctorName, dateTime, status, notes) {
    // Створюємо модальне вікно для відображення деталей
    let modalHtml = `
    <div class="modal fade" id="detailsModal" tabindex="-1" role="dialog" aria-labelledby="detailsModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="detailsModalLabel">Деталі запису</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p><strong>Пацієнт:</strong> ${patientName}</p>
                    <p><strong>Лікар:</strong> ${doctorName}</p>
                    <p><strong>Дата та час:</strong> ${dateTime}</p>
                    <p><strong>Статус:</strong> ${status}</p>
                    <p><strong>Причина:</strong> ${notes || 'Не вказано'}</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Закрити</button>
                </div>
            </div>
        </div>
    </div>
    `;

    // Додаємо модальне вікно до body
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Показуємо модальне вікно
    $('#detailsModal').modal('show');

    // Видаляємо модальне вікно після закриття
    $('#detailsModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function handleEditClick(appointmentId, patientId, doctorId, dateTime, status, notes) {
    // Форматуємо дату для поля вводу
    const dateObj = new Date(dateTime);
    const formattedDate = dateObj.toISOString().slice(0, 16); // формат "yyyy-MM-ddThh:mm"

    // Отримуємо списки лікарів та пацієнтів через AJAX
    // Для спрощення, припустимо, що у нас є доступ до цих списків

    // Створюємо модальне вікно для редагування
    let modalHtml = `
    <div class="modal fade" id="editModal" tabindex="-1" role="dialog" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editModalLabel">Редагувати запис</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form id="editAppointmentForm">
                        <input type="hidden" id="appointmentId" value="${appointmentId}">
                        
                        <div class="form-group">
                            <label for="editDateTime">Дата та час:</label>
                            <input type="datetime-local" class="form-control" id="editDateTime" value="${formattedDate}" required>
                        </div>
                        
                        <div class="form-group">
                            <label for="editNotes">Причина:</label>
                            <textarea class="form-control" id="editNotes">${notes || ''}</textarea>
                        </div>
                        
                        <div class="form-group">
                            <label for="editStatus">Статус:</label>
                            <select class="form-control" id="editStatus">
                                <option value="Scheduled" ${status === 'Scheduled' ? 'selected' : ''}>Заплановано</option>
                                <option value="Completed" ${status === 'Completed' ? 'selected' : ''}>Завершено</option>
                                <option value="Cancelled" ${status === 'Cancelled' ? 'selected' : ''}>Скасовано</option>
                                <option value="NoShow" ${status === 'NoShow' ? 'selected' : ''}>Не з'явився</option>
                            </select>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Скасувати</button>
                    <button type="button" class="btn btn-primary" onclick="saveAppointment()">Зберегти</button>
                </div>
            </div>
        </div>
    </div>
    `;

    // Додаємо модальне вікно до body
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Показуємо модальне вікно
    $('#editModal').modal('show');

    // Видаляємо модальне вікно після закриття
    $('#editModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function saveAppointment() {
    // Отримуємо дані з форми
    const appointmentId = document.getElementById('appointmentId').value;
    const dateTime = document.getElementById('editDateTime').value;
    const notes = document.getElementById('editNotes').value;
    const status = document.getElementById('editStatus').value;

    // Відправляємо дані на сервер
    fetch(`/Appointments/Edit/${appointmentId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            // Додайте антифоргери токен, якщо потрібно
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify({
            AppointmentId: appointmentId,
            AppointmentDateTime: dateTime,
            Notes: notes,
            Status: status,
            // Тут потрібно передати також PatientId і DoctorId, але їх потрібно отримати з форми або з серверного боку
        })
    })
        .then(response => {
            if (response.ok) {
                // Закриваємо модальне вікно
                $('#editModal').modal('hide');
                // Оновлюємо сторінку
                location.reload();
            } else {
                alert('Помилка при збереженні запису');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Помилка при збереженні запису');
        });
}

function handleCancelClick(appointmentId) {
    // Створюємо модальне вікно для підтвердження
    let modalHtml = `
    <div class="modal fade" id="cancelModal" tabindex="-1" role="dialog" aria-labelledby="cancelModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="cancelModalLabel">Підтвердження скасування</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p>Ви впевнені, що хочете скасувати цей запис?</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Ні</button>
                    <button type="button" class="btn btn-danger" onclick="confirmCancel(${appointmentId})">Так, скасувати</button>
                </div>
            </div>
        </div>
    </div>
    `;

    // Додаємо модальне вікно до body
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Показуємо модальне вікно
    $('#cancelModal').modal('show');

    // Видаляємо модальне вікно після закриття
    $('#cancelModal').on('hidden.bs.modal', function (e) {
        $(this).remove();
    });
}

function confirmCancel(appointmentId) {
    // Відправляємо запит на сервер для скасування запису
    fetch(`/Appointments/Cancel/${appointmentId}`, {
        method: 'GET'
    })
        .then(response => {
            if (response.ok) {
                // Закриваємо модальне вікно
                $('#cancelModal').modal('hide');
                // Оновлюємо сторінку або змінюємо статус запису в UI
                location.reload();
            } else {
                alert('Помилка при скасуванні запису');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            alert('Помилка при скасуванні запису');
        });
}
