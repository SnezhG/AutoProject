import React from "react";
import Modal from "react-bootstrap/Modal";
import Button from "react-bootstrap/Button";

const DeleteConfirmationModal = ({ show, onHide, onConfirm, confirmationMessage }) => {
    return (
        <Modal show={show} onHide={onHide}>
            <Modal.Header closeButton>
                <Modal.Title>Подтверждение действия</Modal.Title>
            </Modal.Header>
            <Modal.Body>{confirmationMessage}</Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={onHide}>
                    Отмена
                </Button>
                <Button variant="danger" onClick={onConfirm}>
                    Подтвердить
                </Button>
            </Modal.Footer>
        </Modal>
    )
}

export default DeleteConfirmationModal