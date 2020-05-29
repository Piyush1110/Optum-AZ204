function createSampleDocument(documentToCreate) {
    var accepted = __.createDocument(
        __.getSelfLink(),
        documentToCreate,
        (error, documentCreated) => {
            getContext().getResponse().setBody(documentCreated.id)
        }
    );
    if (!accepted) return;
}
